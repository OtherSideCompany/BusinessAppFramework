using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;


namespace OtherSideCore.Adapter.Views
{
   public abstract class ModuleViewModel : ViewViewModelBase
   {
      #region Fields

      private ModuleDescription _moduleDescription;

      #endregion

      #region Properties

      public ModuleDescription ModuleDescription
      {
         get => _moduleDescription;
         set => SetProperty(ref _moduleDescription, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleViewModel(ILoggerFactory loggerFactory, 
                             IUserContext userContext, 
                             IUserDialogService userDialogService) : 
         base(loggerFactory, 
              userContext, 
              userDialogService)
      {

      }

      #endregion

      #region Public Methods

      public override void Dispose()
      {

      }

      #endregion

      #region Private Methods



      #endregion
   }
}
