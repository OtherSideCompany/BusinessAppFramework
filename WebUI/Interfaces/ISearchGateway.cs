using Application.Search;
using Application.Trees;

namespace WebUI.Interfaces
{
   public interface ISearchGateway<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      Task<TSearchResult?> GetSearchResultAsync(int domainObjectId);
      Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest);
      Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest);
      Task<NodeSummary?> GetNodeSummaryAsync(int id);
   }
}
