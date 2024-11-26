using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Adapter.Views
{
   public abstract class ViewBaseViewModel : ObservableObject, IDisposable
   {
      #region Fields

      

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewBaseViewModel()
      {
         
      }

      #endregion

      #region Public Methods

      public abstract Task InitializeAsync();

      public abstract void Dispose();

      #endregion

      #region Private Methods

      protected virtual void NotifyCommandsCanExecuteChanged()
      {

      }

      #endregion
   }
}
