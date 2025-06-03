using OtherSideCore.Wpf.Services;

namespace OtherSideCore.Presentation
{
   public interface IPresentationModule
   {
      PresentationDescription PresentationDescription { get; }
      List<IPresentationWorkspace> Workspaces { get; }
      void RegisterViews(IViewLocatorService viewLocator);
   }
}
