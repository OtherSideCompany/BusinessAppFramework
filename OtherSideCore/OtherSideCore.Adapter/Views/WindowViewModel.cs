using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.AppConfiguration;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter.Views
{
   public class WindowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      protected readonly ILoggerFactory _loggerFactory;
      protected readonly IGlobalDataService _globalDataService;
      protected IUserContext _userContext;
      protected IAppConfiguration _appConfiguration;
      protected IWindowService _windowService;

      private string _applicationLogoImageSource;
      private string _companyLogoImageSource;
      private string _applicationName;
      private string _windowName;

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

      public string WindowName
      {
         get => _windowName;
         set => SetProperty(ref _windowName, value);
      }

      public IUserContext UserContext
      {
         get => _userContext;
         set => SetProperty(ref _userContext, value);
      }      

      public IWindowService WindowService => _windowService;
      public string UserContextFirstName => UserContext?.FirstName;
      public string UserContextLastName => UserContext?.LastName;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowViewModel(IUserContext userContext,
                             IGlobalDataService globalDataService,
                             IAppConfiguration appConfiguration,
                             IWindowService windowService)
      {
         UserContext = userContext;
         _globalDataService = globalDataService;
         _appConfiguration = appConfiguration;
         _windowService = windowService;

         ApplicationName = "Unnamed App";
         WindowName = "Unnamed Window";
      }

      #endregion

      #region Public Methods      
      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
