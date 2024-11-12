using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Views;

namespace OtherSideCore.Adapter.Views
{
   public abstract class WorkspaceViewModel : ViewBaseViewModel
   {
      #region Fields

      private WorkspaceDescription _workspaceDescription;
      private Workspace _workspace;

      #endregion

      #region Properties

      public WorkspaceDescription WorkspaceDescription
      {
         get => _workspaceDescription;
         set => SetProperty(ref _workspaceDescription, value);
      }

      public Workspace Workspace => (Workspace)_viewBase;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      protected WorkspaceViewModel(Workspace workspace, IWindowService windowService) :  base(workspace, windowService)
      {

      }

      #endregion

      #region Public Methods      



      #endregion

      #region Private Methods



      #endregion

   }
}
