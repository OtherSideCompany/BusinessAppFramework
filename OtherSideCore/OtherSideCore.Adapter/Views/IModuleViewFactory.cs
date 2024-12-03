using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Views
{
   public interface IModuleViewFactory
   {
      object CreateView(ViewDescriptionBase viewDescription);
   }
}
