using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Application.Factories
{
    public interface IDomainObjectQueryServiceFactory
    {
        IDomainObjectQueryService<TSearchResult> CreateDomainObjectQueryService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
    }
}
