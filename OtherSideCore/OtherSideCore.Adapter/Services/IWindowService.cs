using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Domain;

namespace OtherSideCore.Adapter.Services
{
    public interface IWindowService
    {
        public void ConfigureService(Type windowViewModelType);
        public void ConfigureMainWindow(object window);
        void CloseWindow(object window);
        void HideTopModal();
        IWindowSession DisplayView(StringKey key, string viewName, object viewModel, DisplayType displayType);
        void ShowDomainObjectReferenceSelectors(List<DomainObjectReferenceSelectorViewModel> domainObjectReferenceSelectorViewModels, DisplayType displayType);
    }
}
