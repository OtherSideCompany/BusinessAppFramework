using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class ModelObjectListSearchViewModel : ViewModelBase
   {
      #region Fields

      private ModelObjectListSearch m_ModelObjectListSearch;
      private MultiTextFilterViewModel m_MultiTextFilterViewModel;

      private ObservableCollection<ModelObjectViewModel> m_SearchResultViewModels;

      private Command m_SearchCommand;
      private Command m_SelectModelObjectCommand;

      #endregion

      #region Properties

      public ModelObjectListSearch ModelObjectListSearch
      {
         get
         {
            return m_ModelObjectListSearch;
         }
         set
         {
            if (value != m_ModelObjectListSearch)
            {
               m_ModelObjectListSearch = value;
               OnPropertyChanged(nameof(ModelObjectListSearch));
            }
         }
      }

      public MultiTextFilterViewModel MultiTextFilterViewModel
      {
         get
         {
            return m_MultiTextFilterViewModel;
         }
         set
         {
            if (value != m_MultiTextFilterViewModel)
            {
               m_MultiTextFilterViewModel = value;
               OnPropertyChanged(nameof(MultiTextFilterViewModel));
            }
         }
      }

      public ObservableCollection<ModelObjectViewModel> SearchResultViewModels
      {
         get
         {
            return m_SearchResultViewModels;
         }
         set
         {
            if (value != m_SearchResultViewModels)
            {
               m_SearchResultViewModels = value;
               OnPropertyChanged(nameof(SearchResultViewModels));
            }
         }
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

      public Command SearchCommand
      {
         get
         {
            if (m_SearchCommand == null)
            {
               m_SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
            }
            return m_SearchCommand;
         }
      }

      public Command SelectModelObjectCommand
      {
         get
         {
            if (m_SelectModelObjectCommand == null)
            {
               m_SelectModelObjectCommand = new Command(ExecuteSelectModelObjectCommand, CanExecuteSelectModelObjectCommand);
            }
            return m_SelectModelObjectCommand;
         }
      }

      #endregion

      #region Constructor

      public ModelObjectListSearchViewModel(ModelObjectListSearch modelObjectListSearch)
      {
         ModelObjectListSearch = modelObjectListSearch;
         MultiTextFilterViewModel = new MultiTextFilterViewModel(ModelObjectListSearch.MultiTextFilter);
         SearchResultViewModels = new ObservableCollection<ModelObjectViewModel>();
      }

      #endregion

      #region Methods

      protected abstract void ConstructSearchResultViewModels();

      public void AddSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Add(modelObjectViewModel);
      }

      public void RemoveSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         SearchResultViewModels.Remove(modelObjectViewModel);
         OnPropertyChanged(nameof(SelectedSearchResultViewModel));
      }

      public void SelectSearchResult(ModelObjectViewModel modelObjectViewModel)
      {
         UnselectSearchResult();

         if (modelObjectViewModel != null)
         {
            modelObjectViewModel.IsSelected = true;
            ModelObjectListSearch.SelectModelObject(modelObjectViewModel.ModelObject);            
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
         }
      }

      private bool CanExecuteSearchCommand(object parameter)
      {
         return true;
      }

      private void ExecuteSearchCommand(object parameter)
      {
         UnselectSearchResult();
         ModelObjectListSearch.Search();
         SearchResultViewModels.Clear();
         ConstructSearchResultViewModels();
      }

      private bool CanExecuteSelectModelObjectCommand(object parameter)
      {
         var modelObjectViewModel = parameter as ModelObjectViewModel;
         return modelObjectViewModel != null && ModelObjectListSearch.CanSelectModelObject(modelObjectViewModel.ModelObject);
      }

      private void ExecuteSelectModelObjectCommand(object parameter)
      {
         var modelObjectViewModel = parameter as ModelObjectViewModel;

         SelectSearchResult(modelObjectViewModel);
      }

      private void UnselectSearchResult()
      {
         if (SelectedSearchResultViewModel != null)
         {
            SelectedSearchResultViewModel.IsSelected = false;
         }
      }

      public override void Dispose()
      {
         foreach (var viewModel in SearchResultViewModels)
         {
            viewModel.Dispose();
         }

         SearchResultViewModels.Clear();

         ModelObjectListSearch.Dispose();
         MultiTextFilterViewModel.Dispose();
      }

      #endregion
   }
}
