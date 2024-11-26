using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application;

namespace OtherSideCore.Adapter.Views
{
    public abstract class ModuleViewModel : ViewBaseViewModel
   {
      #region Fields

      private ModuleDescription _moduleDescription;
      private Module _module;

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

      public ModuleViewModel(Module module) : base()
      {
         _module = module;
      }

      #endregion


      #region Private Methods



      #endregion
   }
}
