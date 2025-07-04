using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.AppConfiguration;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Adapter.Views
{
    public class WindowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      protected readonly ILoggerFactory _loggerFactory;
      
      protected IUserContext _userContext;
      protected IAppConfiguration _appConfiguration;
      protected IWindowService _windowService;

      private string _applicationLogoImageSource;
      private string _companyLogoImageSource;
      private string _applicationName;

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

      public IUserContext UserContext
      {
         get => _userContext;
         set => SetProperty(ref _userContext, value);
      }      

      public IWindowService WindowService => _windowService;
      public string? UserContextName => UserContext?.GetName();

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowViewModel(IUserContext userContext,
                             IAppConfiguration appConfiguration,
                             IWindowService windowService)
      {
         UserContext = userContext;
         
         _appConfiguration = appConfiguration;
         _windowService = windowService;

         ApplicationName = "Unnamed App";
      }

      #endregion

      #region Public Methods      
      public virtual void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
