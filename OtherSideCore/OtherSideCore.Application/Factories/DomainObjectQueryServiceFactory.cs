using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
    public abstract class DomainObjectQueryServiceFactory : IDomainObjectQueryServiceFactory
    {
        protected ISearchServiceFactory _searchServiceFactory;

        public DomainObjectQueryServiceFactory(ISearchServiceFactory searchServiceFactory)
        {
         _searchServiceFactory = searchServiceFactory;
        }

        public abstract IDomainObjectQueryService<TSearchResult> CreateDomainObjectQueryService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
    }
}
