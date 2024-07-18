using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.ViewModel
{
    public class RepositoryEditorViewModel<T> : ObservableObject, IRepositoryEditorViewModel where T : ModelObject, new()
   {
      #region Fields

      private List<DatabaseField> _databaseFields;
      private IRepositoryFactory _repositoryFactory;
      private User _authenticatedUser;
      private RepositorySearch<T> _repositorySearch;

      private bool m_IsSelectionLocked;
      private Func<CancellationToken, Task> m_SelectedSearchResultChangedAsync;

      private IModelObjectViewModelFactory _modelObjectViewModeFactory;
      
      private MultiTextFilterViewModel _multiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_searchResultViewModels;
      private CollectionViewSource _searchResultViewModelsCollection;

      #endregion

      #region Properties

      public bool IsAnyDatabaseFieldDirty
      {
         get
         {
            return _databaseFields.Any(dbf => dbf.IsDirty);
         }
      }

      public bool IsSelectionLocked
      {
         get => m_IsSelectionLocked;
         set => SetProperty(ref m_IsSelectionLocked, value);
      }

      public Func<CancellationToken, Task> SelectedSearchResultChangedAsync
      {
         get => m_SelectedSearchResultChangedAsync;
         set => SetProperty(ref m_SelectedSearchResultChangedAsync, value);
      }

      public MultiTextFilterViewModel MultiTextFilterViewModel
      {
         get => _multiTextFilterViewModel;
         set => SetProperty(ref _multiTextFilterViewModel, value);
      }

      public ObservableCollection<ModelObjectViewModel> SearchResultViewModels
      {
         get => m_searchResultViewModels;
         set => SetProperty(ref m_searchResultViewModels, value);
      }

      public ICollectionView SearchResultViewModelsCollection
      {
         get => _searchResultViewModelsCollection.View;
      }

      public ModelObjectViewModel SelectedSearchResultViewModel
      {
         get
         {
            return SearchResultViewModels.FirstOrDefault(vm => vm.IsSelected);
         }
      }

      #endregion

      #region Commands

      public AsyncRelayCommand SearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }
      public AsyncRelayCommand<ModelObjectViewModel> SelectSearchResultCommandAsync { get; private set; }
      public RelayCommand CancelSelectSearchResultCommand { get; private set; }
      public AsyncRelayCommand CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveSelectedSearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelSelectedSearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand DeleteSelectedSearchResultAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public RepositoryEditorViewModel(IRepositoryFactory repositoryFactory, User autenticatedUser, IModelObjectViewModelFactory modelObjectViewModeFactory)
      {
         _modelObjectViewModeFactory = modelObjectViewModeFactory;
         _databaseFields = new List<DatabaseField>();
         _repositoryFactory = repositoryFactory;
         _authenticatedUser = autenticatedUser;
         _repositorySearch = new RepositorySearch<T>(repositoryFactory.CreateRepository<T>());

         SearchCommandAsync = new AsyncRelayCommand(SearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);
         SelectSearchResultCommandAsync = new AsyncRelayCommand<ModelObjectViewModel>(SelectSearchResultAsync, CanSelectSearchResult);
         CancelSelectSearchResultCommand = new RelayCommand(CancelSelectSearchResult);
         CreateAsyncCommand = new AsyncRelayCommand(CreateAsync);
         SaveSelectedSearchResultChangesAsyncCommand = new AsyncRelayCommand(SaveSelectedSearchResultChangesAsync, CanSaveSelectedSearchResultChanges);
         CancelSelectedSearchResultChangesAsyncCommand = new AsyncRelayCommand(CancelSelectedSearchResultChangesAsync, CanCancelSelectedSearchResultChanges);
         DeleteSelectedSearchResultAsyncCommand = new AsyncRelayCommand(DeleteSelectedSearchResultAsync, CanExecuteDeleteSelectedSearchResult);
         
         MultiTextFilterViewModel = new MultiTextFilterViewModel(_repositorySearch.MultiTextFilter, SearchCommandAsync);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
         _searchResultViewModelsCollection = new CollectionViewSource();
         _searchResultViewModelsCollection.Source = SearchResultViewModels;
      }

      #endregion

      #region Methods

      private void RegisterDatabaseFields()
      {
         UnregisterDatabaseFields();

         foreach (var databaseField in SelectedSearchResultViewModel.ModelObject.GetDatabaseFields())
         {
            databaseField.PropertyChanged += DatabaseField_OnPropertyChanged;
            _databaseFields.Add(databaseField);
         }
      }

      private void DatabaseField_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DatabaseField.IsDirty)))
         {
            OnPropertyChanged(nameof(IsAnyDatabaseFieldDirty));

            NotifyCommandsCanExecuteChanged();

            if (IsAnyDatabaseFieldDirty)
            {
               LockSelection();
            }
            else
            {
               UnlockSelection();
            }
         }
      }

      private void UnregisterDatabaseFields()
      {
         foreach (var databaseField in _databaseFields)
         {
            databaseField.PropertyChanged -= DatabaseField_OnPropertyChanged;
         }

         _databaseFields.Clear();

         OnPropertyChanged(nameof(IsAnyDatabaseFieldDirty));
      }

      public void LockSelection()
      {
         IsSelectionLocked = true;
      }

      public void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      public void NotifyCommandsCanExecuteChanged()
      {
         SearchCommandAsync.NotifyCanExecuteChanged();
         CancelSearchCommand.NotifyCanExecuteChanged();
         SelectSearchResultCommandAsync.NotifyCanExecuteChanged();
         CancelSelectSearchResultCommand.NotifyCanExecuteChanged();
         CreateAsyncCommand.NotifyCanExecuteChanged();
         SaveSelectedSearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelSelectedSearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         DeleteSelectedSearchResultAsyncCommand.NotifyCanExecuteChanged();
      }

      protected List<ModelObjectViewModel> ConstructSearchResultViewModels()
      {
         var viewModels = new List<ModelObjectViewModel>();

         foreach (var searchResult in _repositorySearch.SearchResults)
         {
            var viewModel = _modelObjectViewModeFactory.CreateViewModel(searchResult as T);
            viewModels.Add(viewModel);
         }

         return viewModels;
      }

      private void UnloadSearchResultViewModels()
      {
         foreach (var viewModel in SearchResultViewModels)
         {
            viewModel.Dispose();
         }

         SearchResultViewModels.Clear();
      }

      public void AddSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Add(modelObjectViewModel);
      }

      public void RemoveSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Remove(modelObjectViewModel);
         OnPropertyChanged(nameof(SelectedSearchResultViewModel));
      }

      public bool CanSelectSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         return !IsSelectionLocked && modelObjectViewModel != null && !modelObjectViewModel.IsSelected;
      }

      private void UnselectSearchResult()
      {
         if (SelectedSearchResultViewModel != null)
         {
            SelectedSearchResultViewModel.IsSelected = false;
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
            UnregisterDatabaseFields();
         }

         NotifyCommandsCanExecuteChanged();
      }

      private async Task SelectSearchResultAsync(ModelObjectViewModel modelObjectViewModel, CancellationToken cancellationToken)
      {
         try
         {
            UnselectSearchResult();

            if (modelObjectViewModel != null)
            {
               modelObjectViewModel.IsSelected = true;
               OnPropertyChanged(nameof(SelectedSearchResultViewModel));

               RegisterDatabaseFields();

               if (SelectedSearchResultChangedAsync != null)
               {
                  await SelectedSearchResultChangedAsync(cancellationToken);
               }
            }
         }
         catch (OperationCanceledException)
         {
            UnselectSearchResult();
         }

         NotifyCommandsCanExecuteChanged();
      }

      private async Task SearchAsync(CancellationToken cancellationToken)
      {
         try
         {
            UnloadSearchResultViewModels();

            await _repositorySearch.SearchAsync(cancellationToken);
            var viewModels = ConstructSearchResultViewModels();

            foreach (var viewModel in viewModels)
            {
               SearchResultViewModels.Add(viewModel);
            }
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResultViewModels();
         }
      }

      private void CancelSearch()
      {
         SearchCommandAsync.Cancel();
      }

      private void CancelSelectSearchResult()
      {
         SelectSearchResultCommandAsync.Cancel();
      }

      private async Task CreateAsync()
      {
         if (!IsSelectionLocked)
         {
            UnselectSearchResult();
         }

         var wasSelectionLocked = IsSelectionLocked;

         LockSelection();

         using var repository = _repositoryFactory.CreateRepository<T>();
         var modelObject = await repository.CreateAsync(_authenticatedUser.Id.Value);

         var viewModel = _modelObjectViewModeFactory.CreateViewModel(modelObject);
         AddSearchResult(viewModel);

         if (!wasSelectionLocked)
         {
            await SelectSearchResultAsync(viewModel, CancellationToken.None);
            UnlockSelection();
         }         
      }

      public virtual bool CanSaveSelectedSearchResultChanges()
      {
         return SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.ModelObject.CanSaveChanges();
      }

      public virtual async Task SaveSelectedSearchResultChangesAsync()
      {
         using var repository = _repositoryFactory.CreateRepository<T>();
         await repository.SaveAsync(SelectedSearchResultViewModel.ModelObject as T, _authenticatedUser.Id.Value);
      }

      public virtual bool CanCancelSelectedSearchResultChanges()
      {
         return SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.ModelObject.CanCancelChanges();
      }

      public virtual async Task CancelSelectedSearchResultChangesAsync()
      {
         using var repository = _repositoryFactory.CreateRepository<T>();
         await repository.LoadAsync(SelectedSearchResultViewModel.ModelObject as T);
      }

      private bool CanExecuteDeleteSelectedSearchResult()
      {
         return SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.ModelObject.CanBeDeleted();
      }

      private async Task DeleteSelectedSearchResultAsync()
      {
         var result = MessageBox.Show("Confirmez-vous la suppression ? ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

         if (result == MessageBoxResult.Yes)
         {
            LockSelection();

            using var repository = _repositoryFactory.CreateRepository<T>();
            await repository.DeleteAsync(SelectedSearchResultViewModel.ModelObject as T);

            var selectedSearchResultViewModel = SelectedSearchResultViewModel;            
            SelectedSearchResultViewModel.Dispose();
            UnselectSearchResult();
            RemoveSearchResult(selectedSearchResultViewModel);

            UnlockSelection();

            NotifyCommandsCanExecuteChanged();
         }
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         UnregisterDatabaseFields();

         _repositorySearch.Dispose();

         MultiTextFilterViewModel.Dispose();
      }

      #endregion
   }
}
