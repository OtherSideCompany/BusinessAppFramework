using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using OtherSideCore.Model;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class MainWindowViewModel<T, U> : ObservableObject, IDisposable where T : User where U : DbContext
   {
      #region Fields

      protected IAuthenticationService<T> _authenticationService;
      protected IDbContextFactory<U> _contextFactory;

      private string _applicationLogoImageSource;
      private string _applicationName;
      private bool _isNavigationMenuDisplayed;
      private List<ViewBase> _views;
      private List<ViewBase> _quickNavigationViews;
      private IDisposable _instanciatedViewModel;
      private System.Windows.Controls.UserControl _mainWindowContent;

      #endregion

      #region Properties

      public IAuthenticationService<T> AuthenticationService
      {
         get => _authenticationService;
         set => SetProperty(ref _authenticationService, value);
      }

      public string ApplicationLogoImageSource
      {
         get => _applicationLogoImageSource;
         set => SetProperty(ref _applicationLogoImageSource, value);
      }

      public string ApplicationName
      {
         get => _applicationName;
         set => SetProperty(ref _applicationName, value);
      }

      public bool IsNavigationMenuDisplayed
      {
         get => _isNavigationMenuDisplayed;
         set => SetProperty(ref _isNavigationMenuDisplayed, value);
      }

      public List<ViewBase> Views
      {
         get => _views;
         set => SetProperty(ref _views, value);
      }

      public List<ViewBase> QuickNavigationViews
      {
         get => _quickNavigationViews;
         set => SetProperty(ref _quickNavigationViews, value);
      }

      public IDisposable InstanciatedViewModel
      {
         get => _instanciatedViewModel;
         set => SetProperty(ref _instanciatedViewModel, value);
      }

      public System.Windows.Controls.UserControl MainWindowContent
      {
         get => _mainWindowContent;
         set => SetProperty(ref _mainWindowContent, value);
      }

      public ViewBase LoadedView => Views.FirstOrDefault(v => v.IsLoaded);

      #endregion

      #region Commands

      public AsyncRelayCommand AuthenticateUserAsyncCommand { get; set; }
      public AsyncRelayCommand DisconnectAuthenticatedUserAsyncCommand { get; set; }
      public RelayCommand<ViewBase> DisplayViewCommand { get; set; }

      #endregion

      #region Constructor

      public MainWindowViewModel(IAuthenticationService<T> authenticationService, IDbContextFactory<U> contextFactory)
      {
         AuthenticationService = authenticationService;
         _contextFactory = contextFactory;

         Views = new List<ViewBase>();
         QuickNavigationViews = new List<ViewBase>();

         AuthenticateUserAsyncCommand = new AsyncRelayCommand(AuthenticateUserAsync);
         DisconnectAuthenticatedUserAsyncCommand = new AsyncRelayCommand(DisconnectAuthenticatedUserAsync);
         DisplayViewCommand = new RelayCommand<ViewBase>(DisplayView);

         ApplicationName = "Unnamed App";
      }

      #endregion

      #region Methods

      public async Task AuthenticateUserAsync()
      {
         if (AuthenticationService.CanAuthenticateUser())
         {
            await AuthenticationService.AuthenticateUserAsync(null, "");
         }
      }

      private async Task DisconnectAuthenticatedUserAsync()
      {
         if (AuthenticationService.CanLogoutUser())
         {
            await AuthenticationService.LogoutUserAsync();
         }
      }

      private void DisplayView(ViewBase viewBase)
      {
         if (InstanciatedViewModel != null)
         {
            InstanciatedViewModel.Dispose();
         }

         Views.ForEach(v => v.IsLoaded = false);

         viewBase.IsLoaded = true;

         InstanciatedViewModel = viewBase is ViewGroup ? viewBase : (IDisposable)Activator.CreateInstance(viewBase.ViewModelType);

         OnPropertyChanged(nameof(LoadedView));
      }

      public void Dispose()
      {
         
      }

      #endregion
   }
}
