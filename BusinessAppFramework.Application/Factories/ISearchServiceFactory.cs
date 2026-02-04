using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Services;

namespace BusinessAppFramework.Application.Factories
{
   public interface ISearchServiceFactory
   {
      ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
      object CreateSearchService(Type domainObjectSearchResultType);
   }
}
