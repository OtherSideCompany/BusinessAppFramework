using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
   public interface ISearchServiceFactory
   {
      ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();
   }
}
