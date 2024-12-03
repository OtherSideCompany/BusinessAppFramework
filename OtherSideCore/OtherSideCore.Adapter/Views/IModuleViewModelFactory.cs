using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Views
{
   public interface IModuleViewModelFactory
   {
      ViewBaseViewModel CreateViewModel(ViewDescriptionBase viewDescription);
   }
}
