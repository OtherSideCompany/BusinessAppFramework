using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;

namespace BusinessAppFramework.Application.Factories
{
   public interface ISearchServiceFactory
   {
      ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
      object CreateSearchService(Type domainObjectSearchResultType);
   }
}
