
using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectViewModelSelection : ObservableObject
   {
      #region Fields

      private DomainObjectViewModel _selectedViewModel;
      private List<DomainObjectViewModel> _selectedViewModels;

      #endregion

      #region Properties

      public DomainObjectViewModelSelectionType SelectionType { get; private set; }

      public DomainObjectViewModel SelectedViewModel
      {
         get
         {
            if (SelectionType == DomainObjectViewModelSelectionType.None)
            {
               return null;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Single)
            {
               return _selectedViewModel;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Multiple)
            {
               return _selectedViewModels.FirstOrDefault();
            }
            else
            {
               throw new ArgumentException("Unknown selection type " + SelectionType.ToString());
            }
         }
      }

      public List<DomainObjectViewModel> SelectedViewModels
      {
         get
         {
            if (SelectionType == DomainObjectViewModelSelectionType.None)
            {
               return null;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Single)
            {
               return null;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Multiple)
            {
               return _selectedViewModels;
            }
            else
            {
               throw new ArgumentException("Unknown selection type " + SelectionType.ToString());
            }
         }
      }

      public bool IsSelectionEmpty
      {
         get
         {
            if (SelectionType == DomainObjectViewModelSelectionType.None)
            {
               return true;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Single)
            {
               return _selectedViewModel == null;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Multiple)
            {
               return !_selectedViewModels.Any();
            }
            else
            {
               return true;
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectViewModelSelection(DomainObjectViewModelSelectionType selectionType)
      {
         SelectionType = selectionType;
         _selectedViewModels = new List<DomainObjectViewModel>();
      }

      #endregion

      #region Public Methods

      public void SetSelectionType(DomainObjectViewModelSelectionType selectionType)
      {
         SelectionType = selectionType;
      }

      public void SelectViewModel(DomainObjectViewModel viewModel)
      {
         if (viewModel != null)
         {
            if (SelectionType == DomainObjectViewModelSelectionType.Single && SelectedViewModel != viewModel)
            {
               UnselectViewModel(SelectedViewModel);
               _selectedViewModel = viewModel;
               _selectedViewModel.IsSelected = true;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Multiple && !_selectedViewModels.Contains(viewModel))
            {
               if (_selectedViewModel == null)
               {
                  _selectedViewModel = viewModel;
               }

               _selectedViewModels.Add(viewModel);
               viewModel.IsSelected = true;
            }

            NotifyPropertyChanged();
         }
      }

      public void UnselectViewModel(DomainObjectViewModel viewModel)
      {
         if (viewModel != null)
         {
            if (SelectionType == DomainObjectViewModelSelectionType.Single && viewModel == _selectedViewModel)
            {
               viewModel.IsSelected = false;
               _selectedViewModel = null;
            }
            else if (SelectionType == DomainObjectViewModelSelectionType.Multiple && _selectedViewModels.Contains(viewModel))
            {
               viewModel.IsSelected = false;
               _selectedViewModels.Remove(viewModel);

               if (viewModel.Equals(_selectedViewModel))
               {
                  _selectedViewModel = _selectedViewModels.FirstOrDefault();
               }
            }

            NotifyPropertyChanged();
         }
      }

      public void ClearSelection()
      {
         if (_selectedViewModel != null)
         {
            _selectedViewModel.IsSelected = false;
         }

         if (_selectedViewModels != null)
         {
            _selectedViewModels.ForEach(vm => vm.IsSelected = false);
         }

         _selectedViewModel = null;
         _selectedViewModels?.Clear();

         NotifyPropertyChanged();
      }

      #endregion

      #region Private Methods

      private void NotifyPropertyChanged()
      {
         OnPropertyChanged(nameof(SelectedViewModel));
         OnPropertyChanged(nameof(SelectedViewModels));
         OnPropertyChanged(nameof(IsSelectionEmpty));
      }

      #endregion
   }
}
