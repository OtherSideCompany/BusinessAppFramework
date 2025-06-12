using OtherSideCore.Presentation.Services;

namespace OtherSideCore.Presentation
{
   public abstract class PresentationModule : IPresentationModule
   {
      #region Fields

      

      #endregion

      #region Properties

      public PresentationDescription PresentationDescription { get; set; }

      public List<IPresentationWorkspace> Workspaces { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public PresentationModule()
      {
         Workspaces = new List<IPresentationWorkspace>();
         PresentationDescription = new PresentationDescription();
      }      

      #endregion

      #region Public Methods

      public abstract void RegisterViews(IViewLocatorService viewLocator);

      public abstract IDisposable GetViewModel();

      #endregion

      #region Private Methods



      #endregion
   }
}
