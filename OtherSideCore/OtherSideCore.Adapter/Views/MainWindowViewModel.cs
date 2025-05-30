using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.ViewDescriptions;
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
      private IModuleViewModelFactory _viewModelFactory;
      private IDomainObjectInteractionService domainObjectInteractionFactory;
      private IUserDialogService _userDialogService;
      protected readonly IGlobalDataService _globalDataService;

      private bool _isNavigationMenuDisplayed;
      private List<ViewDescriptionBase> _viewDescriptions;
      private List<ViewDescriptionBase> _quickNavigationViewescriptions;
      private IDisposable _instanciatedViewModel;
      private ViewDescriptionBase _defaultViewDescription;
      private ViewBaseViewModel _loadedViewViewModel;
      private bool _isLoadingContent;

      private string _connexionUserName;
      private string _connexionPassword;
      private bool _rememberUserName;
      private bool _isUserContextInitialized;

      #endregion

      #region Properties      

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

      public IWindowService WindowService => _windowService;
      public string UserContextFirstName => UserContext?.FirstName;
      public string UserContextLastName => UserContext?.LastName;
      public ViewDescriptionBase LoadedViewDescription => ViewDescriptions.FirstOrDefault(vd => vd.IsLoaded);

      #endregion

      #region Events

      public EventHandler LoadedViewModelChanged;

      #endregion

      #region Commands

      public AsyncRelayCommand LogInAsyncCommand { get; set; }
      public AsyncRelayCommand LogOutAsyncCommand { get; set; }
      public AsyncRelayCommand<ViewDescriptionBase> DisplayViewAsyncCommand { get; set; }

      #endregion

      #region Constructor

      public MainWindowViewModel(IUserDialogService userDialogService,
                                 IAuthenticationService authenticationService,
                                 IUserContext userContext,
                                 IGlobalDataService globalDataService,
                                 IAppConfiguration appConfiguration,
                                 IWindowService windowService,
                                 IModuleViewModelFactory viewModelFactory) :
         base(userContext,
              appConfiguration,
              windowService)
      {
         _authenticationService = authenticationService;
         _viewModelFactory = viewModelFactory;
         _userDialogService = userDialogService;
         _globalDataService = globalDataService;

         ViewDescriptions = new List<ViewDescriptionBase>();
         QuickNavigationViewDescriptions = new List<ViewDescriptionBase>();

         LogInAsyncCommand = new AsyncRelayCommand(LogInAsync);
         LogOutAsyncCommand = new AsyncRelayCommand(LogOutAsync);
         DisplayViewAsyncCommand = new AsyncRelayCommand<ViewDescriptionBase>(DisplayViewAsync);

         _appConfiguration.Load();
         LoadSettings();

         WindowName = "";
      }

      #endregion

      #region Public Methods      

      public void Dispose()
      {
         base.Dispose();

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
                  await DisplayViewAsync(DefaultViewDescription);
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

      public async Task DisplayViewAsync(ViewDescriptionBase viewDescriptionBase)
      {
         IsLoadingContent = true;

         var proceed = true;

         if (LoadedViewViewModel is WorkspaceViewModel workspaceViewModel && workspaceViewModel.HasUnsavedChanges)
         {
            proceed = _userDialogService.Confirm("Vous avez des modifications non sauvegardées. Voulez-vous continuer ?");
         }

         if (proceed)
         {
            LoadedViewViewModel?.Dispose();
            LoadedViewViewModel = null;

            ViewDescriptions.ForEach(vd => vd.Unload());

            viewDescriptionBase.Load();

            LoadedViewViewModel = _viewModelFactory.CreateViewModel(LoadedViewDescription);

            await LoadedViewViewModel.InitializeAsync();

            if (viewDescriptionBase is ModuleDescription)
            {
               ((ModuleViewModel)LoadedViewViewModel).ModuleDescription = (ModuleDescription)viewDescriptionBase;
            }
            else if (viewDescriptionBase is WorkspaceDescription)
            {
               ((WorkspaceViewModel)LoadedViewViewModel).WorkspaceDescription = (WorkspaceDescription)viewDescriptionBase;
            }

            OnPropertyChanged(nameof(LoadedViewDescription));
            WindowName = LoadedViewDescription.ViewNavigationPath;
         }

         LoadedViewModelChanged?.Invoke(this, new EventArgs());

         IsLoadingContent = false;
      }

      #endregion
   }
}
