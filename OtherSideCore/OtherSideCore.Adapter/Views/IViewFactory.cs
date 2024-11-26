using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Views
{
   public interface IViewFactory
   {
      object CreateView(ViewDescriptionBase viewDescription);
   }
}
