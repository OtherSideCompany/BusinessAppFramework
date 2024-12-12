using OtherSideCore.Adapter.ViewDescriptions;

namespace OtherSideCore.Adapter.Factories
{
    public interface IModuleViewFactory
    {
        object CreateView(ViewDescriptionBase viewDescription);
    }
}
