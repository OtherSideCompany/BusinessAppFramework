using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.Views
{
   public class NavigationMenuViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private ObservableCollection<NavigationItem> _navigationItems;
      private NavigationItem? _selectedNavigationItem;

      #endregion

      #region Properties

      public ObservableCollection<NavigationItem> NavigationItems
      {
         get => _navigationItems; 
         set => SetProperty(ref _navigationItems, value); 
      }

      public NavigationItem? SelectedNavigationItem
      {
         get => _selectedNavigationItem;
         set => SetProperty(ref _selectedNavigationItem, value);
      }

      #endregion

      #region Events

      public event EventHandler<NavigationItem?> NavigationItemSelected;

      #endregion

      #region Commands

      public RelayCommand<NavigationItem?> SelectNavigationItemCommand { get; private set; }

      #endregion

      #region Constructor

      public NavigationMenuViewModel(List<NavigationItem> navigationItems)
      {
         NavigationItems = new ObservableCollection<NavigationItem>(navigationItems);

         SelectNavigationItemCommand = new RelayCommand<NavigationItem?>(SelectNavigationItem);
      }

      #endregion

      #region Public Methods

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods

      private void SelectNavigationItem(NavigationItem? navigationItem)
      {
         if (navigationItem != null)
         {
            UnselectAllNavigationItems();

            SelectedNavigationItem = navigationItem;
            SelectedNavigationItem.IsSelected = true;
            NavigationItemSelected?.Invoke(this, _selectedNavigationItem);
         }
      }

      private void UnselectAllNavigationItems()
      {
         NavigationItems.ToList().ForEach(m => m.IsSelected = false);
      }

      #endregion
   }
}
