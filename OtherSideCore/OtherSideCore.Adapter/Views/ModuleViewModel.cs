using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Views;


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

      public Module Module => (Module)_viewBase;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleViewModel(Module module) : base(module)
      {
         _module = module;
      }

      #endregion


      #region Private Methods



      #endregion
   }
}
