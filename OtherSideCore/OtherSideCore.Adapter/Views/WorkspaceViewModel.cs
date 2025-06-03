using OtherSideCore.Adapter.DomainObjectInteractionViewModel;

namespace OtherSideCore.Adapter.Views
{
   public abstract class WorkspaceViewModel : ViewBaseViewModel, ISavable
   {
      #region Fields

      protected bool _hasUnsavedChanges;

      #endregion

      #region Properties

      public virtual bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         set { SetProperty(ref _hasUnsavedChanges, value); NotifyCommandsCanExecuteChanged(); }
      }


      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods      

      public virtual bool CanCancelChanges() 
      { 
         return HasUnsavedChanges;
      }

      public abstract Task CancelChangesAsync();

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public abstract Task SaveChangesAsync();

      #endregion

      #region Private Methods



      #endregion

   }
}
