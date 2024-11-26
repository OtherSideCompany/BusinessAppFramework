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

      public WorkspaceDescription(int id, string name, object iconResource, ModuleDescription parent = null) : base(id, name, iconResource)
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
