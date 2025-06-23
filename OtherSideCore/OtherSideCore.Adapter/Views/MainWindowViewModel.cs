using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.AppConfiguration;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter.Views
{
   public abstract class MainWindowViewModel : WindowViewModel
   {
      #region Fields

      private IAuthenticationService _authenticationService;
      private IDomainObjectInteractionService domainObjectInteractionFactory;
      protected IUserDialogService _userDialogService;
      protected readonly IGlobalDataService _globalDataService;

      private IDisposable? _currentViewModel;
      private object? _currentView;

      private NavigationMenuViewModel _navigationMenuViewModel;

      private bool _isLoadingContent;
      private string _connexionUserName;
      private string _connexionPassword;
      private bool _rememberUserName;
      private bool _isUserContextInitialized;

      #endregion

      #region Properties      

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

      public bool IsLoadingContent
      {
         get => _isLoadingContent;
         set => SetProperty(ref _isLoadingContent, value);
      }      

      public bool IsUserContextInitialized
      {
         get => _isUserContextInitialized;
         set
         {
            SetProperty(ref _isUserContextInitialized, value);
            OnPropertyChanged(nameof(UserContextFirstName));
            OnPropertyChanged(nameof(UserContextLastName));
         }
      }

      public IDisposable? CurrentViewModel
      {
         get => _currentViewModel;
         protected set => SetProperty(ref _currentViewModel, value);
      }

      public object? CurrentView
      {
         get => _currentView;
         set => SetProperty(ref _currentView, value);
      }

      public NavigationMenuViewModel NavigationMenuViewModel
      {
         get => _navigationMenuViewModel;
         set => SetProperty(ref _navigationMenuViewModel, value);
      }  

      #endregion      

      #region Commands

      public AsyncRelayCommand LogInAsyncCommand { get; set; }
      public AsyncRelayCommand LogOutAsyncCommand { get; set; }
      #endregion

      #region Constructor

      public MainWindowViewModel(IUserDialogService userDialogService,
                                 IAuthenticationService authenticationService,
                                 IUserContext userContext,
                                 IGlobalDataService globalDataService,
                                 IAppConfiguration appConfiguration,
                                 IWindowService windowService) :
         base(userContext,
              appConfiguration,
              windowService)
      {
         _authenticationService = authenticationService;
         _userDialogService = userDialogService;
         _globalDataService = globalDataService;         

         LogInAsyncCommand = new AsyncRelayCommand(LogInAsync);
         LogOutAsyncCommand = new AsyncRelayCommand(LogOutAsync);

         _appConfiguration.Load();
         LoadSettings();
      }

      #endregion

      #region Public Methods

      public override void Dispose()
      {
         base.Dispose();

         _globalDataService.UnloadData();
      }

      #endregion

      #region Private Methods

      protected abstract Task InitializeUserContextAsync(int userId);

      protected abstract Task ShutdownUserContextAsync();

      private void LoadSettings()
      {
         RememberUserName = _appConfiguration.RememberUserName;

         if (RememberUserName)
         {
            ConnexionUserName = _appConfiguration.UserLogin;
         }
      }

      private void SaveSettings()
      {
         if (RememberUserName)
         {
            _appConfiguration.UserLogin = ConnexionUserName;
            _appConfiguration.RememberUserName = RememberUserName;
         }
         else
         {
            _appConfiguration.UserLogin = string.Empty;
            _appConfiguration.RememberUserName = false;
         }

         _appConfiguration.Save();
      }

      private void ResetConnexionInfos()
      {
         ConnexionUserName = "";
         ConnexionPassword = "";
      }

      private async Task LogInAsync()
      {
         if (!String.IsNullOrEmpty(ConnexionUserName) && !String.IsNullOrEmpty(ConnexionPassword))
         {
            (var areCredentialsValid, int userId) = await _authenticationService.VerifyPasswordAsync(ConnexionUserName, ConnexionPassword);

            if (!areCredentialsValid)
            {
               _userDialogService.Error("Nom d'utilisateur ou mot de passe invalide");
            }
            else
            {
               SaveSettings();

               await InitializeUserContextAsync(userId);
               IsUserContextInitialized = true;

               await _globalDataService.LoadGlobalDataAsync();
               await DisplayDefaultViewAsync();
            }

            ResetConnexionInfos();
         }
         else
         {
            _userDialogService.Error("Veuillez entrer un nom d'utilisateur et un mot de passe valides.");
         }
      }

      protected abstract Task DisplayDefaultViewAsync();

      private async Task LogOutAsync()
      {
         await ShutdownUserContextAsync();

         _globalDataService.UnloadData();
         IsUserContextInitialized = false;

         LoadSettings();
      }

      #endregion
   }
}
