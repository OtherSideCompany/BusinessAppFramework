using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.Application.Services
{
   public interface ISearchService<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      Task<int> CountAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
      Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
      Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default);
      Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest, CancellationToken cancellationToken = default);
      Task<NodeSummary?> GetSummaryAsync(int domainObjectId);
   }
}
