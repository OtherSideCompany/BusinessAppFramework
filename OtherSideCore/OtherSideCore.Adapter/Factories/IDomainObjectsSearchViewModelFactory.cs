using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Factories
{
    public interface IDomainObjectsSearchViewModelFactory
    {
        IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<T>(
           DomainObjectSearch<T> domainObjectSearch,
           IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
           IDomainObjectSearchResultFactory domainObjectSearchResultFactory) where T : DomainObject, new();
    }
}
