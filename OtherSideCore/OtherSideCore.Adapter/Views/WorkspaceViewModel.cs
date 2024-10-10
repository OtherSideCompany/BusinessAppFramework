using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.Views
{
   public abstract class WorkspaceViewModel : ViewViewModelBase
   {
      #region Fields

      private WorkspaceDescription _workspaceDescription;

      #endregion

      #region Properties

      public WorkspaceDescription WorkspaceDescription
      {
         get => _workspaceDescription;
         set => SetProperty(ref _workspaceDescription, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      protected WorkspaceViewModel(ILoggerFactory loggerFactory, IUserContext userContext, IUserDialogService userDialogService, IDomainObjectViewModelFactory viewModelFactory) : base(loggerFactory, userContext, userDialogService, viewModelFactory)
      {

      }

      #endregion

      #region Public Methods      



      #endregion

      #region Private Methods



      #endregion

   }
}
