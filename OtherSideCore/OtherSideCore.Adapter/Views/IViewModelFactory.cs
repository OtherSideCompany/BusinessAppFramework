using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Views
{
   public interface IViewModelFactory
   {
      ViewBaseViewModel CreateViewModel(ViewDescriptionBase viewDescription);
   }
}
