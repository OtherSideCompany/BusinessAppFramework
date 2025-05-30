using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Factories
{
    public interface IDomainObjectsSearchViewModelFactory
    {
        IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<TSearchResult>(
           DomainObjectSearch<TSearchResult> domainObjectSearch,
           IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory) where TSearchResult : DomainObjectSearchResult, new();
    }
}
