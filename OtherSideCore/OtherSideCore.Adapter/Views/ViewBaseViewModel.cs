using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.Views;

namespace OtherSideCore.Adapter.Views
{
   public abstract class ViewBaseViewModel : ObservableObject, IDisposable
   {
      #region Fields

      protected ViewBase _viewBase;
      protected IWindowService _windowService;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewBaseViewModel(ViewBase viewBase, IWindowService windowService)
      {
         _viewBase = viewBase;
         _windowService = windowService;
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
