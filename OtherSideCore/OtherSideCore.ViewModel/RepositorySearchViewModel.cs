using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OtherSideCore.ViewModel
{
   public abstract class RepositorySearchViewModel<T> : ViewModelBase where T : Model.ModelObject
   {
      #region Fields

      private User _authenticatedUser;

      private RepositoryManager<T> _repositorySearch;
      private MultiTextFilterViewModel _multiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_searchResultViewModels;
      private CollectionViewSource _searchResultViewModelsCollection;

      #endregion

      #region Properties

      public User AuthenticatedUser
      {
         get => _authenticatedUser;
         set => SetProperty(ref _authenticatedUser, value);
      }

      public RepositoryManager<T> RepositorySearch
      {
         get => _repositorySearch;
         set => SetProperty(ref _repositorySearch, value);
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

      #endregion

      #region Constructor

      public RepositorySearchViewModel(RepositoryManager<T> repositorySearch, User authenticatedUser)
      {
         SearchCommandAsync = new AsyncRelayCommand(SearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);
         SelectModelObjectCommandAsync = new AsyncRelayCommand<ModelObjectViewModel>(SelectModelObjectAsync, CanSelectModelObject);
         CancelSelectModelObjectCommand = new RelayCommand(CancelSelectModelObject);

         AuthenticatedUser = authenticatedUser;

         RepositorySearch = repositorySearch;
         MultiTextFilterViewModel = new MultiTextFilterViewModel(RepositorySearch.MultiTextFilter, SearchCommandAsync);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
         _searchResultViewModelsCollection = new CollectionViewSource();
         _searchResultViewModelsCollection.Source = SearchResultViewModels;
      }

      #endregion

      #region Methods

      protected abstract Task<List<ModelObjectViewModel>> ConstructSearchResultViewModels();

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
            await RepositorySearch.SelectModelObjectAsync(modelObjectViewModel.ModelObject, cancellationToken);
         }
      }

      private async Task SearchAsync(CancellationToken cancellationToken)
      {
         try
         {
            UnloadSearchResultViewModels();

            cancellationToken.ThrowIfCancellationRequested();

            await RepositorySearch.SearchAsync(cancellationToken);
            var viewModels = await ConstructSearchResultViewModels();

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
         return modelObjectViewModel != null && RepositorySearch.CanSelectModelObject(modelObjectViewModel.ModelObject);
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
      }

      public override void Dispose()
      {
         UnloadSearchResultViewModels();

         RepositorySearch.Dispose();
         MultiTextFilterViewModel.Dispose();
      }

      #endregion
   }
}
