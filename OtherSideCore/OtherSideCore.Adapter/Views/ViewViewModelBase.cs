using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.Views
{
   public abstract class ViewViewModelBase : ObservableObject, IDisposable
   {
      #region Fields

      protected IUserContext _userContext;
      protected ILoggerFactory _loggerFactory;
      protected IUserDialogService _userDialogService;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewViewModelBase(ILoggerFactory loggerFactory,
                               IUserContext userContext,
                               IUserDialogService userDialogService)
      {
         _userContext = userContext;
         _loggerFactory = loggerFactory;
         _userDialogService = userDialogService;
      }

      #endregion

      #region Public Methods

      public abstract Task InitializeAsync(CancellationToken cancellationToken);

      public abstract void Dispose();

      #endregion

      #region Private Methods

      protected virtual void NotifyCommandsCanExecuteChanged()
      {

      }

      #endregion
   }
}
