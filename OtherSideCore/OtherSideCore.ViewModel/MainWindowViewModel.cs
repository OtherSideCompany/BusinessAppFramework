using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OtherSideCore.ViewModel
{
   public abstract class MainWindowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private readonly IServiceProvider _serviceProvider;
      protected IAuthenticationService _authenticationService;

      private string _applicationLogoImageSource;
      private string _companyLogoImageSource;
      private string _applicationName;
      private bool _isNavigationMenuDisplayed;
      private List<ViewDescriptionBase> _viewDescriptions;
      private List<ViewDescriptionBase> _quickNavigationViewescriptions;
      private IDisposable _instanciatedViewModel;
      private System.Windows.Controls.UserControl _mainWindowContent;
      private ViewDescriptionBase _defaultViewDescription;

      private string _connexionUserName;
      private string _connexionPassword;
      private bool _rememberUserName;

      #endregion

      #region Properties

      public IAuthenticationService AuthenticationService
      {
         get => _authenticationService;
         set => SetProperty(ref _authenticationService, value);
      }

      public string ApplicationLogoImageSource
      {
         get => _applicationLogoImageSource;
         set => SetProperty(ref _applicationLogoImageSource, value);
      }

      public string CompanyLogoImageSource
      {
         get => _companyLogoImageSource;
         set => SetProperty(ref _companyLogoImageSource, value);
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

      public List<ViewDescriptionBase> ViewDescriptions
      {
         get => _viewDescriptions;
         set => SetProperty(ref _viewDescriptions, value);
      }

      public List<ViewDescriptionBase> QuickNavigationViewDescriptions
      {
         get => _quickNavigationViewescriptions;
         set => SetProperty(ref _quickNavigationViewescriptions, value);
      }

      public System.Windows.Controls.UserControl MainWindowContent
      {
         get => _mainWindowContent;
         set => SetProperty(ref _mainWindowContent, value);
      }

      public string ConnexionUserName
      {
         get => _connexionUserName;
         set => SetProperty(ref _connexionUserName, value);
      }

      public string ConnexionPassword
      {
         get => _connexionPassword;
         set => SetProperty(ref _connexionPassword, value);
      }

      public bool RememberUserName
      {
         get => _rememberUserName;
         set => SetProperty(ref _rememberUserName, value);
      }

      public ViewDescriptionBase DefaultViewDescription
      {
         get => _defaultViewDescription;
         set => SetProperty(ref _defaultViewDescription, value);
      }

      public ViewDescriptionBase LoadedViewDescription => ViewDescriptions.FirstOrDefault(vd => vd.IsLoaded);

      #endregion

      #region Commands

      public AsyncRelayCommand AuthenticateUserAsyncCommand { get; set; }
      public AsyncRelayCommand DisconnectAuthenticatedUserAsyncCommand { get; set; }
      public RelayCommand<ViewDescriptionBase> DisplayViewCommand { get; set; }

      #endregion

      #region Constructor

      public MainWindowViewModel(IAuthenticationService authenticationService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
      {
         AuthenticationService = authenticationService;
         _serviceProvider = serviceProvider;

         ViewDescriptions = new List<ViewDescriptionBase>();
         QuickNavigationViewDescriptions = new List<ViewDescriptionBase>();

         AuthenticateUserAsyncCommand = new AsyncRelayCommand(AuthenticateUserAsync);
         DisconnectAuthenticatedUserAsyncCommand = new AsyncRelayCommand(DisconnectAuthenticatedUserAsync);
         DisplayViewCommand = new RelayCommand<ViewDescriptionBase>(DisplayView);

         ApplicationName = "Unnamed App";

         LoadSettings();
      }

      #endregion

      #region Public Methods

      public async Task AuthenticateUserAsync()
      {
         if (AuthenticationService.CanAuthenticateUser(ConnexionUserName, ConnexionPassword))
         {
            await AuthenticationService.AuthenticateUserAsync(ConnexionUserName, ConnexionPassword);         
            
            if (!AuthenticationService.IsUserAuthenticated())
            {
               MessageBox.Show("Nom d'utilisateur ou mot de passe invalide", "Erreur d'authentification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
               SaveSettings();

               if (DefaultViewDescription != null)
               {
                  DisplayView(DefaultViewDescription);
               }
            }

            ResetConnexionInfos();
         }
         else
         {
            MessageBox.Show("Veuillez entrer un nom d'utilisateur et un mot de passe valides.", "Erreur d'authentification", MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }      

      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods

      private void LoadSettings()
      {
         RememberUserName = Properties.Settings.Default.RememberUserName;

         if (RememberUserName)
         {
            ConnexionUserName = Properties.Settings.Default.UserName;
         }
      }

      private void SaveSettings()
      {
         if (RememberUserName)
         {
            Properties.Settings.Default.UserName = ConnexionUserName;
            Properties.Settings.Default.RememberUserName = RememberUserName;
         }
         else
         {
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.RememberUserName = false;
         }

         Properties.Settings.Default.Save();
      }

      private void ResetConnexionInfos()
      {
         ConnexionUserName = "";
         ConnexionPassword = "";
      }

      private async Task DisconnectAuthenticatedUserAsync()
      {
         if (AuthenticationService.CanLogoutUser())
         {
            await AuthenticationService.LogoutUserAsync();
         }

         LoadSettings();
      }

      private void DisplayView(ViewDescriptionBase viewBase)
      {
         LoadedViewDescription?.Dispose();

         ViewDescriptions.ForEach(v => v.IsLoaded = false);

         viewBase.IsLoaded = true;

         LoadedViewDescription.InstanciateViewModel();

         OnPropertyChanged(nameof(LoadedViewDescription));
      }

      #endregion
   }
}
