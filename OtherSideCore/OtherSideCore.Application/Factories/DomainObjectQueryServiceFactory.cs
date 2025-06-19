using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Application.Factories
{
   public class DomainObjectQueryServiceFactory : IDomainObjectQueryServiceFactory
   {
      protected ISearchServiceFactory _searchServiceFactory;

      public DomainObjectQueryServiceFactory(ISearchServiceFactory searchServiceFactory)
      {
         _searchServiceFactory = searchServiceFactory;
      }

      public IDomainObjectQueryService<TSearchResult> CreateDomainObjectQueryService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new()
      {
         return new DomainObjectQueryService<TSearchResult>(_searchServiceFactory);
      }
   }
}
