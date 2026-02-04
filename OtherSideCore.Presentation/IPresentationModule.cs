using OtherSideCore.Presentation.Services;

namespace OtherSideCore.Presentation
{
   public interface IPresentationModule
   {
      PresentationDescription PresentationDescription { get; }
      List<IPresentationWorkspace> Workspaces { get; }
      void RegisterViews(IViewLocatorService viewLocator);
      IDisposable GetViewModel();
   }
}
