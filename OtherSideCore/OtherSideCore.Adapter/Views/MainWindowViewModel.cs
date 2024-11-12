using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.AppConfiguration;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter.Views
{
   public abstract class MainWindowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      protected readonly IServiceProvider _serviceProvider;
      protected readonly IUserDialogService _userDialogService;
      protected readonly IAuthenticationService _authenticationService;
      protected readonly ILoggerFactory _loggerFactory;
      protected readonly IGlobalDataService _globalDataService;
      protected IUserContext _userContext;
      protected IAppConfiguration _appConfiguration;
      protected IWindowService _windowService;

      private string _applicationLogoImageSource;
      private string _companyLogoImageSource;
      private string _applicationName;
      private bool _isNavigationMenuDisplayed;
      private List<ViewDescriptionBase> _viewDescriptions;
      private List<ViewDescriptionBase> _quickNavigationViewescriptions;
      private IDisposable _instanciatedViewModel;
      private ViewDescriptionBase _defaultViewDescription;
      private ViewBaseViewModel _loadedViewViewModel;

      private string _connexionUserName;
      private string _connexionPassword;
      private bool _rememberUserName;
      private bool _isUserContextInitialized;

      #endregion

      #region Properties

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

      public ViewBaseViewModel LoadedViewViewModel
      {
         get => _loadedViewViewModel;
         set => SetProperty(ref _loadedViewViewModel, value);
      }

      public IUserContext UserContext
      {
         get => _userContext;
         set => SetProperty(ref _userContext, value);
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

      public IWindowService WindowService => _windowService;

      public string UserContextFirstName => UserContext?.FirstName;
      public string UserContextLastName => UserContext?.LastName;

      public ViewDescriptionBase LoadedViewDescription => ViewDescriptions.FirstOrDefault(vd => vd.IsLoaded);

      #endregion

      #region Commands

      public AsyncRelayCommand LogInAsyncCommand { get; set; }
      public AsyncRelayCommand LogOutAsyncCommand { get; set; }
      public AsyncRelayCommand<ViewDescriptionBase> DisplayViewAsyncCommand { get; set; }

      #endregion

      #region Constructor

      public MainWindowViewModel(IServiceProvider serviceProvider, 
                                 IUserDialogService userDialogService, 
                                 IAuthenticationService authenticationService, 
                                 IUserContext userContext,
                                 IGlobalDataService globalDataService,
                                 IAppConfiguration appConfiguration,
                                 IWindowService windowService)
      {
         _userDialogService = userDialogService;
         _authenticationService = authenticationService;
         _serviceProvider = serviceProvider;
         UserContext = userContext;
         _globalDataService = globalDataService;
         _appConfiguration = appConfiguration;
         _windowService = windowService;

         ViewDescriptions = new List<ViewDescriptionBase>();
         QuickNavigationViewDescriptions = new List<ViewDescriptionBase>();

         LogInAsyncCommand = new AsyncRelayCommand(LogInAsync);
         LogOutAsyncCommand = new AsyncRelayCommand(LogOutAsync);
         DisplayViewAsyncCommand = new AsyncRelayCommand<ViewDescriptionBase>(DisplayViewAsync);

         ApplicationName = "Unnamed App";

         _appConfiguration.Load();
         LoadSettings();
      }

      #endregion

      #region Public Methods      

      public void Dispose()
      {
         _globalDataService.UnloadData();
         ViewDescriptions.ForEach(vd => vd.Dispose());
         QuickNavigationViewDescriptions.ForEach(vd => vd.Dispose());
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

               if (DefaultViewDescription != null)
               {
                  await InitializeUserContextAsync(userId);
                  IsUserContextInitialized = true;

                  await _globalDataService.LoadGlobalDataAsync();
                  await DisplayViewAsync(DefaultViewDescription, CancellationToken.None);                  
               }
            }

            ResetConnexionInfos();
         }
         else
         {
            _userDialogService.Error("Veuillez entrer un nom d'utilisateur et un mot de passe valides.");
         }

      }

      private async Task LogOutAsync()
      {
         await ShutdownUserContextAsync();

         _globalDataService.UnloadData();
         IsUserContextInitialized = false;

         LoadSettings();
      }

      private async Task DisplayViewAsync(ViewDescriptionBase viewDescriptionBase, CancellationToken cancellationToken)
      {
         LoadedViewViewModel?.Dispose();

         ViewDescriptions.ForEach(vd => vd.Unload());

         viewDescriptionBase.Load();

         LoadedViewViewModel = (ViewBaseViewModel)_serviceProvider.GetService(LoadedViewDescription.ViewModelType);
         await LoadedViewViewModel.InitializeAsync(cancellationToken);

         if (viewDescriptionBase is ModuleDescription)
         {
            ((ModuleViewModel)LoadedViewViewModel).ModuleDescription = (ModuleDescription)viewDescriptionBase;
         }

         OnPropertyChanged(nameof(LoadedViewDescription));
      }

      #endregion
   }
}
