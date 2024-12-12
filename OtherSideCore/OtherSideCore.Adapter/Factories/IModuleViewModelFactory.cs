using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Adapter.Views;

namespace OtherSideCore.Adapter.Factories
{
    public interface IModuleViewModelFactory
    {
        ViewBaseViewModel CreateViewModel(ViewDescriptionBase viewDescription);
    }
}
