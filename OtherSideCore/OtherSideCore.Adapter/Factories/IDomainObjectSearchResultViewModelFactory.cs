using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Adapter.Factories
{
    public interface IDomainObjectSearchResultViewModelFactory
    {
        DomainObjectSearchResultViewModel CreateViewModel(DomainObjectSearchResult domainObjectSearchResult);
    }
}
