using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Application.Services;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.Views
{
   public class NavigationMenuViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private IUserPermissionResolverService _userPermissionResolverService;

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

      public NavigationMenuViewModel(
         List<NavigationItem> navigationItems,
         IUserPermissionResolverService userPermissionResolverService)
      {
         _userPermissionResolverService = userPermissionResolverService;

         NavigationItems = new ObservableCollection<NavigationItem>(navigationItems);

         SelectNavigationItemCommand = new RelayCommand<NavigationItem?>(SelectNavigationItem);
      }

      #endregion

      #region Public Methods

      public void UnselectAllNavigationItems()
      {
         NavigationItems.ToList().ForEach(m => m.IsSelected = false);
      }

      public async Task FilterNavigationItemsForUser(int userId)
      {
         foreach (var navigationItem in NavigationItems)
         {
            var permissionKeys = WorkspacePermissionKeysHelper.GetPermissionKeys(navigationItem.ViewModelType);
            navigationItem.IsVisible = await _userPermissionResolverService.CanAccessAnyAsync(permissionKeys, userId);
         }
      }

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

      #endregion
   }
}
