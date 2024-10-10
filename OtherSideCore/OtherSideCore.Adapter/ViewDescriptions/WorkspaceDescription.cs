using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.Views;

namespace OtherSideCore.Adapter.ViewDescriptions
{
   public class WorkspaceDescription : ViewDescriptionBase
   {
      #region Fields

      private ModuleDescription _parentModule;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WorkspaceDescription(string name, Type viewModelType, object iconResource, ModuleDescription parent = null) : base(name, viewModelType, iconResource)
      {
         _parentModule = parent;

         if (_parentModule != null)
         {
            ViewNavigationPath = _parentModule.Name + " > " + Name;
         }
         else
         {
            ViewNavigationPath = Name;
         }
      }

      #endregion

      #region Public Methods



      #endregion
   }
}
