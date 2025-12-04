using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Services
{
   public interface ISearchService<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      Task<int> CountAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
      Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
      Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default);
      Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest, CancellationToken cancellationToken = default);
   }
}
