using OtherSideCore.Wpf.Services;

namespace OtherSideCore.Presentation
{
   public interface IPresentationModule
   {
      void RegisterViews(IViewLocatorService viewLocator);
   }
}
