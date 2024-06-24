using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class ModelObjectListSearchViewModel : ViewModelBase
   {
      #region Fields

      private ModelObjectListSearch m_ModelObjectListSearch;
      private MultiTextFilterViewModel m_MultiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_SearchResultViewModels;

      #endregion

      #region Properties

      public ModelObjectListSearch ModelObjectListSearch
      {
         get => m_ModelObjectListSearch;
         set => SetProperty(ref m_ModelObjectListSearch, value);
      }

      public MultiTextFilterViewModel MultiTextFilterViewModel
      {
         get => m_MultiTextFilterViewModel;
         set => SetProperty(ref m_MultiTextFilterViewModel, value);
      }

      public ObservableCollection<ModelObjectViewModel> SearchResultViewModels
      {
         get => m_SearchResultViewModels;
         set => SetProperty(ref m_SearchResultViewModels, value);
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

      public AsyncRelayCommand SearchCommand { get; private set; }
      public AsyncRelayCommand<ModelObjectViewModel> SelectModelObjectCommand { get; private set; }

      #endregion

      #region Constructor

      public ModelObjectListSearchViewModel(ModelObjectListSearch modelObjectListSearch)
      {
         SearchCommand = new AsyncRelayCommand(SearchAsync);
         SelectModelObjectCommand = new AsyncRelayCommand<ModelObjectViewModel>(SelectModelObjectAsync, CanSelectModelObject);

         ModelObjectListSearch = modelObjectListSearch;
         MultiTextFilterViewModel = new MultiTextFilterViewModel(ModelObjectListSearch.MultiTextFilter);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
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

      public async Task SelectSearchResultAsync(ModelObjectViewModel modelObjectViewModel)
      {
         UnselectSearchResult();

         if (modelObjectViewModel != null)
         {
            modelObjectViewModel.IsSelected = true;
            await ModelObjectListSearch.SelectModelObjectAsync(modelObjectViewModel.ModelObject);            
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
         }
      }

      private async Task SearchAsync()
      {         
         UnloadSearchResultViewModels();

         await ModelObjectListSearch.SearchAsync();
         var viewModels = await ConstructSearchResultViewModels();

         foreach (var viewModel in viewModels)
         {
            SearchResultViewModels.Add(viewModel);
         }
      }

      private bool CanSelectModelObject(ModelObjectViewModel modelObjectViewModel)
      {
         return modelObjectViewModel != null && ModelObjectListSearch.CanSelectModelObject(modelObjectViewModel.ModelObject);
      }

      private async Task SelectModelObjectAsync(ModelObjectViewModel modelObjectViewModel)
      {
         await SelectSearchResultAsync(modelObjectViewModel);
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

         ModelObjectListSearch.Dispose();
         MultiTextFilterViewModel.Dispose();
      }

      #endregion
   }
}
