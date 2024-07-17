using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
   public class RepositoryManagerViewModel<T> : ObservableObject, IRepositoryManagerViewModel where T : Model.ModelObject, new()
   {
      #region Fields

      private User _authenticationUser;
      private IModelObjectViewModeFactory _modelObjectViewModeFactory;

      private RepositoryManager<T> _repositoryManager;
      private MultiTextFilterViewModel _multiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_searchResultViewModels;
      private CollectionViewSource _searchResultViewModelsCollection;

      #endregion

      #region Properties

      public RepositoryManager<T> RepositoryManager
      {
         get => _repositoryManager;
         set => SetProperty(ref _repositoryManager, value);
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
      public AsyncRelayCommand<ModelObjectViewModel> SelectModelObjectCommandAsync { get; private set; }
      public RelayCommand CancelSelectModelObjectCommand { get; private set; }
      public AsyncRelayCommand CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveSelectedObjectChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelSelectedObjectChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand DeleteSelectedObjectAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public RepositoryManagerViewModel(RepositoryManager<T> repositoryManager, User autenticatedUser, IModelObjectViewModeFactory modelObjectViewModeFactory)
      {
         _authenticationUser = autenticatedUser;
         _modelObjectViewModeFactory = modelObjectViewModeFactory;

         SearchCommandAsync = new AsyncRelayCommand(SearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);
         SelectModelObjectCommandAsync = new AsyncRelayCommand<ModelObjectViewModel>(SelectModelObjectAsync, CanSelectModelObject);
         CancelSelectModelObjectCommand = new RelayCommand(CancelSelectModelObject);
         CreateAsyncCommand = new AsyncRelayCommand(CreateAsync);
         SaveSelectedObjectChangesAsyncCommand = new AsyncRelayCommand(SaveSelectedObjectChangesAsync, CanSaveSelectedObjectChanges);
         CancelSelectedObjectChangesAsyncCommand = new AsyncRelayCommand(CancelSelectedObjectChangesAsync, CanCancelSelectedObjectChanges);
         DeleteSelectedObjectAsyncCommand = new AsyncRelayCommand(DeleteSelectedObjectAsync, CanExecuteDeleteSelectedObject);

         RepositoryManager = repositoryManager;
         MultiTextFilterViewModel = new MultiTextFilterViewModel(RepositoryManager.MultiTextFilter, SearchCommandAsync);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
         _searchResultViewModelsCollection = new CollectionViewSource();
         _searchResultViewModelsCollection.Source = SearchResultViewModels;
      }

      #endregion

      #region Methods

      public void NotifyCommandsCanExecuteChanged()
      {
         SearchCommandAsync.NotifyCanExecuteChanged();
         CancelSearchCommand.NotifyCanExecuteChanged();
         SelectModelObjectCommandAsync.NotifyCanExecuteChanged();
         CancelSelectModelObjectCommand.NotifyCanExecuteChanged();
         CreateAsyncCommand.NotifyCanExecuteChanged();
         SaveSelectedObjectChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelSelectedObjectChangesAsyncCommand.NotifyCanExecuteChanged();
         DeleteSelectedObjectAsyncCommand.NotifyCanExecuteChanged();
      }

      protected List<ModelObjectViewModel> ConstructSearchResultViewModels()
      {
         var viewModels = new List<ModelObjectViewModel>();

         foreach (var searchResult in RepositoryManager.SearchResults)
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

      public async Task SelectSearchResultAsync(ModelObjectViewModel modelObjectViewModel, CancellationToken cancellationToken)
      {
         UnselectSearchResult();

         if (modelObjectViewModel != null)
         {
            modelObjectViewModel.IsSelected = true;
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
            await RepositoryManager.SelectModelObjectAsync(modelObjectViewModel.ModelObject, cancellationToken);
         }

         NotifyCommandsCanExecuteChanged();
      }

      private async Task SearchAsync(CancellationToken cancellationToken)
      {
         try
         {
            UnloadSearchResultViewModels();

            cancellationToken.ThrowIfCancellationRequested();

            await RepositoryManager.SearchAsync(cancellationToken);
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

      private bool CanSelectModelObject(ModelObjectViewModel modelObjectViewModel)
      {
         return modelObjectViewModel != null && RepositoryManager.CanSelectModelObject(modelObjectViewModel.ModelObject);
      }

      private async Task SelectModelObjectAsync(ModelObjectViewModel modelObjectViewModel, CancellationToken cancellationToken)
      {
         try
         {
            await SelectSearchResultAsync(modelObjectViewModel, cancellationToken);
         }
         catch (OperationCanceledException)
         {
            UnselectSearchResult();
         }
      }

      private void CancelSelectModelObject()
      {
         SelectModelObjectCommandAsync.Cancel();
      }

      public void UnselectSearchResult()
      {
         if (SelectedSearchResultViewModel != null)
         {
            SelectedSearchResultViewModel.IsSelected = false;
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
         }

         NotifyCommandsCanExecuteChanged();
      }

      private async Task CreateAsync()
      {
         RepositoryManager.UnselectModelObject();
         RepositoryManager.LockSelection();

         var modelObject = RepositoryManager.CreateModelObjectInstance();

         await RepositoryManager.Repository.SaveAsync(modelObject, _authenticationUser.Id.Value);
         await RepositoryManager.SelectModelObjectAsync(modelObject, CancellationToken.None);

         var viewModel = _modelObjectViewModeFactory.CreateViewModel(modelObject);
         SearchResultViewModels.Add(viewModel);

         RepositoryManager.UnlockSelection();
      }

      public virtual bool CanSaveSelectedObjectChanges()
      {
         return RepositoryManager.SelectedModelObject != null && RepositoryManager.SelectedModelObject.CanSaveChanges();
      }

      public virtual async Task SaveSelectedObjectChangesAsync()
      {
         await RepositoryManager.Repository.SaveAsync(RepositoryManager.SelectedModelObject, _authenticationUser.Id.Value);
         await RepositoryManager.Repository.LoadAsync(RepositoryManager.SelectedModelObject);
      }

      public virtual bool CanCancelSelectedObjectChanges()
      {
         return RepositoryManager.SelectedModelObject != null && RepositoryManager.SelectedModelObject.CanCancelChanges();
      }

      public virtual async Task CancelSelectedObjectChangesAsync()
      {
         await RepositoryManager.Repository.LoadAsync(RepositoryManager.SelectedModelObject);
      }

      private bool CanExecuteDeleteSelectedObject()
      {
         return RepositoryManager.SelectedModelObject != null && RepositoryManager.SelectedModelObject.CanBeDeleted();
      }

      private async Task DeleteSelectedObjectAsync()
      {
         var result = MessageBox.Show("Confirmez-vous la suppression ? ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

         if (result == MessageBoxResult.Yes)
         {
            RepositoryManager.LockSelection();

            await RepositoryManager.Repository.DeleteAsync(RepositoryManager.SelectedModelObject);
            RepositoryManager.RemoveSearchResult(RepositoryManager.SelectedModelObject);

            SelectedSearchResultViewModel.Dispose();
            RemoveSearchResult(SelectedSearchResultViewModel);

            RepositoryManager.UnlockSelection();

            NotifyCommandsCanExecuteChanged();
         }
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         MultiTextFilterViewModel.Dispose();
      }

      #endregion
   }
}
