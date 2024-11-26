using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Views
{
   public abstract class WorkspaceViewModel : ViewBaseViewModel
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

      public virtual bool HasUnsavedChanges => false;

      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods      



      #endregion

      #region Private Methods



      #endregion

   }
}
