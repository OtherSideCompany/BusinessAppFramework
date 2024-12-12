using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Browser;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class MultiTextFilterViewModel : ObservableObject
   {
      #region Fields

      private ObservableCollection<TextFilterViewModel> _filters;

      #endregion

      #region Properties

      public ObservableCollection<TextFilterViewModel> Filters
      {
         get => _filters;
         set => SetProperty(ref _filters, value);
      }

      #endregion

      #region Events



      #endregion

      #region Commands

      public RelayCommand<TextFilterViewModel> RemoveFilterCommand { get; private set; }
      public RelayCommand ClearFiltersCommand { get; private set; }
      public RelayCommand AddFilterCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel()
      {
         Filters = new ObservableCollection<TextFilterViewModel>();

         RemoveFilterCommand = new RelayCommand<TextFilterViewModel>(RemoveTextFilter);
         ClearFiltersCommand = new RelayCommand(ClearFilters);
         AddFilterCommand = new RelayCommand(AddFilter);
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      private void RemoveTextFilter(TextFilterViewModel textFilter)
      {
         Filters.Remove(textFilter);
      }

      private void ClearFilters()
      {
         Filters.Clear();
      }

      private void AddFilter()
      {
         Filters.Add(new TextFilterViewModel());
      }

      #endregion
   }
}
