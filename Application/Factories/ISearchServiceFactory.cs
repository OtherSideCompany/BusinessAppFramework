using Application.Search;
using Application.Services;

namespace Application.Factories
{
   public interface ISearchServiceFactory
   {
      ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
      object CreateSearchService(Type domainObjectSearchResultType);
   }
}
