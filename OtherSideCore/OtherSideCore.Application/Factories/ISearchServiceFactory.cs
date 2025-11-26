using OtherSideCore.Application.Repository;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Application.Factories
{
   public interface ISearchServiceFactory
   {
      ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
   }
}
