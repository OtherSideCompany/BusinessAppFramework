using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DatabaseFields;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.ViewModel.RepositoryEditorViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
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
      protected IRepositoryFactory _repositoryFactory;
      protected User _authenticatedUser;
      private RepositorySearch<T> _repositorySearch;
      private ObservableCollection<ConstraintViewModel> _constraintViewModels;

      private bool m_IsSelectionLocked;
      private Func<CancellationToken, Task> m_SelectedSearchResultChangedAsync;

      protected IModelObjectViewModelFactory _modelObjectViewModelFactory;

      private MultiTextFilterViewModel _multiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_searchResultViewModels;
      private CollectionViewSource _searchResultViewModelsCollection;

      #endregion

      #region Properties

      public bool IsAnyDatabaseFieldDirty
      {
         get
         {
            var dirtydbfields = _databaseFields.Where(dbf => dbf.IsDirty);
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

      public ObservableCollection<ConstraintViewModel> ConstraintViewModels
      {
         get => _constraintViewModels;
         set => SetProperty(ref _constraintViewModels, value);
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

      public bool IsAnyConstraintSelected
      {
         get
         {
            return ConstraintViewModels.Any(vm => vm.IsSelected);
         }
      }

      #endregion

      #region Commands

      public AsyncRelayCommand<bool> SearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }
      public AsyncRelayCommand<ModelObjectViewModel> SelectSearchResultCommandAsync { get; private set; }
      public RelayCommand CancelSelectSearchResultCommand { get; private set; }
      public AsyncRelayCommand CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveSelectedSearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveDirtySearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelSelectedSearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelDirtySearchResultChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand DeleteSelectedSearchResultAsyncCommand { get; private set; }
      public AsyncRelayCommand<ModelObject> DeleteAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public RepositoryEditorViewModel(IRepositoryFactory repositoryFactory, User autenticatedUser, IModelObjectViewModelFactory modelObjectViewModelFactory)
      {
         _modelObjectViewModelFactory = modelObjectViewModelFactory;
         _databaseFields = new List<DatabaseField>();
         _repositoryFactory = repositoryFactory;
         _authenticatedUser = autenticatedUser;
         _repositorySearch = new RepositorySearch<T>(repositoryFactory.CreateRepository<T>());

         SearchCommandAsync = new AsyncRelayCommand<bool>(SearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);
         SelectSearchResultCommandAsync = new AsyncRelayCommand<ModelObjectViewModel>(SelectSearchResultAsync, CanSelectSearchResult);
         CancelSelectSearchResultCommand = new RelayCommand(CancelSelectSearchResult);
         CreateAsyncCommand = new AsyncRelayCommand(CreateAsync);
         SaveSelectedSearchResultChangesAsyncCommand = new AsyncRelayCommand(SaveSelectedSearchResultChangesAsync, CanSaveSelectedSearchResultChanges);
         SaveDirtySearchResultChangesAsyncCommand = new AsyncRelayCommand(SaveDirtySearchResultChangesAsync, CanSaveDirtySearchResultChanges);
         CancelSelectedSearchResultChangesAsyncCommand = new AsyncRelayCommand(CancelSelectedSearchResultChangesAsync, CanCancelSelectedSearchResultChanges);
         CancelDirtySearchResultChangesAsyncCommand = new AsyncRelayCommand(CancelDirtySearchResultChangesAsync, CanCancelDirtySearchResultChanges);
         DeleteSelectedSearchResultAsyncCommand = new AsyncRelayCommand(DeleteSelectedSearchResultAsync, CanExecuteDeleteSelectedSearchResult);
         DeleteAsyncCommand = new AsyncRelayCommand<ModelObject>(DeleteAsync, CanExecuteDelete);

         ConstraintViewModels = new ObservableCollection<ConstraintViewModel>();
         MultiTextFilterViewModel = new MultiTextFilterViewModel(new MultiTextFilter(true), SearchCommandAsync);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
         _searchResultViewModelsCollection = new CollectionViewSource();
         _searchResultViewModelsCollection.Source = SearchResultViewModels;
      }

      #endregion

      #region Public Methods

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         UnregisterDatabaseFields();

         _repositorySearch.Dispose();

         MultiTextFilterViewModel.Dispose();
      }

      public void SetConstraints(List<Constraint> constraints)
      {
         ConstraintViewModels.Clear();

         foreach (var constraint in constraints)
         {
            ConstraintViewModels.Add(new ConstraintViewModel(constraint));
         }

         OnPropertyChanged(nameof(IsAnyConstraintSelected));
      }

      public void ActivateConstraint(Constraint constraint)
      {
         foreach (var constraintViewModel in ConstraintViewModels)
         {
            constraintViewModel.IsSelected = constraintViewModel.Constraint.Equals(constraint);
         }

         OnPropertyChanged(nameof(IsAnyConstraintSelected));
      }

      public async Task SearchAsync(bool extendedSearch, CancellationToken cancellationToken)
      {
         try
         {
            var selectedModelObject = SelectedSearchResultViewModel?.ModelObject;

            UnselectSearchResult();

            UnloadSearchResultViewModels();

            await _repositorySearch.SearchAsync(MultiTextFilterViewModel.MultiTextFilter.StringFilters,
                                                ConstraintViewModels.Where(vm => vm.IsSelected).Select(vm => vm.Constraint).ToList(),
                                                extendedSearch,
                                                cancellationToken);

            var viewModels = ConstructSearchResultViewModels();

            foreach (var viewModel in viewModels)
            {
               SearchResultViewModels.Add(viewModel);
            }

            RegisterDatabaseFields();

            if (selectedModelObject != null)
            {
               await SelectSearchResultAsync(SearchResultViewModels.FirstOrDefault(vm => vm.ModelObject.Equals(selectedModelObject)), cancellationToken);
            }
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResultViewModels();
         }
      }

      #endregion

      #region Private Methods

      private void RegisterDatabaseFields()
      {
         UnregisterDatabaseFields();

         foreach (var databaseField in SearchResultViewModels.SelectMany(vm => vm.ModelObject.GetDatabaseFields()))
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

      private void LockSelection()
      {
         IsSelectionLocked = true;
      }

      private void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         SearchCommandAsync.NotifyCanExecuteChanged();
         CancelSearchCommand.NotifyCanExecuteChanged();
         SelectSearchResultCommandAsync.NotifyCanExecuteChanged();
         CancelSelectSearchResultCommand.NotifyCanExecuteChanged();
         CreateAsyncCommand.NotifyCanExecuteChanged();
         SaveSelectedSearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelSelectedSearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelDirtySearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         DeleteSelectedSearchResultAsyncCommand.NotifyCanExecuteChanged();
         SaveDirtySearchResultChangesAsyncCommand.NotifyCanExecuteChanged();
         DeleteAsyncCommand.NotifyCanExecuteChanged();
      }

      protected List<ModelObjectViewModel> ConstructSearchResultViewModels()
      {
         var viewModels = new List<ModelObjectViewModel>();

         foreach (var searchResult in _repositorySearch.SearchResults)
         {
            var viewModel = _modelObjectViewModelFactory.CreateViewModel(searchResult as T);
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

      private void AddSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Add(modelObjectViewModel);

         RegisterDatabaseFields();
      }

      private void RemoveSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Remove(modelObjectViewModel);
         OnPropertyChanged(nameof(SelectedSearchResultViewModel));

         RegisterDatabaseFields();
      }

      private bool CanSelectSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         return !IsSelectionLocked && modelObjectViewModel != null && !modelObjectViewModel.IsSelected;
      }

      private void UnselectSearchResult()
      {
         if (SelectedSearchResultViewModel != null)
         {
            SelectedSearchResultViewModel.IsSelected = false;
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
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

         var viewModel = _modelObjectViewModelFactory.CreateViewModel(modelObject);
         AddSearchResult(viewModel);

         if (!wasSelectionLocked)
         {
            await SelectSearchResultAsync(viewModel, CancellationToken.None);
            UnlockSelection();
         }
      }

      public async Task CreateModelObjectAsync(ModelObject modelObject)
      {
         if (!IsSelectionLocked)
         {
            UnselectSearchResult();
         }

         var wasSelectionLocked = IsSelectionLocked;

         LockSelection();

         using var repository = _repositoryFactory.CreateRepository<T>();
         await repository.SaveAsync((T)modelObject, _authenticatedUser.Id.Value);

         var viewModel = _modelObjectViewModelFactory.CreateViewModel(modelObject);
         AddSearchResult(viewModel);

         if (!wasSelectionLocked)
         {
            await SelectSearchResultAsync(viewModel, CancellationToken.None);
            UnlockSelection();
         }
      }

      protected virtual bool CanSaveSelectedSearchResultChanges()
      {
         return SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.ModelObject.CanSaveChanges();
      }

      protected virtual async Task SaveSelectedSearchResultChangesAsync()
      {
         using var repository = _repositoryFactory.CreateRepository<T>();
         await repository.SaveAsync(SelectedSearchResultViewModel.ModelObject as T, _authenticatedUser.Id.Value);
      }

      protected virtual bool CanSaveDirtySearchResultChanges()
      {
         return IsAnyDatabaseFieldDirty && 
                SearchResultViewModels.Where(vm => vm.ModelObject.GetDatabaseFields().Any(dbf => dbf.IsDirty)).All(vm => vm.ModelObject.CanSaveChanges());
      }

      protected virtual async Task SaveDirtySearchResultChangesAsync()
      {
         foreach (var dirtySearchResultViewModel in SearchResultViewModels.Where(vm => vm.ModelObject.GetDatabaseFields().Any(dbf => dbf.IsDirty)))
         {
            using var repository = _repositoryFactory.CreateRepository<T>();
            await repository.SaveAsync(dirtySearchResultViewModel.ModelObject as T, _authenticatedUser.Id.Value);
         }
      }           

      protected virtual bool CanCancelSelectedSearchResultChanges()
      {
         return SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.ModelObject.CanCancelChanges();
      }

      protected virtual async Task CancelSelectedSearchResultChangesAsync()
      {
         using var repository = _repositoryFactory.CreateRepository<T>();
         await repository.LoadAsync(SelectedSearchResultViewModel.ModelObject as T);
      }

      protected virtual bool CanCancelDirtySearchResultChanges()
      {
         return IsAnyDatabaseFieldDirty &&
                SearchResultViewModels.Where(vm => vm.ModelObject.GetDatabaseFields().Any(dbf => dbf.IsDirty)).All(vm => vm.ModelObject.CanCancelChanges());
      }

      protected virtual async Task CancelDirtySearchResultChangesAsync()
      {
         foreach (var dirtySearchResultViewModel in SearchResultViewModels.Where(vm => vm.ModelObject.GetDatabaseFields().Any(dbf => dbf.IsDirty)))
         {
            using var repository = _repositoryFactory.CreateRepository<T>();
            await repository.LoadAsync(dirtySearchResultViewModel.ModelObject as T);
         }
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

      protected virtual bool CanExecuteDelete(ModelObject modelObject)
      {
         return modelObject != null && modelObject.CanBeDeleted();
      }

      private async Task DeleteAsync(ModelObject modelObject)
      {
         var result = MessageBox.Show("Confirmez-vous la suppression ? ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

         if (result == MessageBoxResult.Yes)
         {
            LockSelection();

            using var repository = _repositoryFactory.CreateRepository<T>();
            await repository.DeleteAsync(modelObject as T);

            var selectedSearchResultViewModel = SearchResultViewModels.FirstOrDefault(vm => vm.ModelObject.Equals(modelObject));
            RemoveSearchResult(selectedSearchResultViewModel);

            UnlockSelection();

            NotifyCommandsCanExecuteChanged();
         }
      }

      #endregion
   }
}
