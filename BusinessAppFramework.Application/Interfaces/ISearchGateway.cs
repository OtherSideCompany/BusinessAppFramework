using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchGateway<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        Task<TSearchResult?> GetSearchResultAsync(int domainObjectId);
        Task<List<TSearchResult>> GetSearchResultsAsync(List<int> domainObjectIds);
        Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest);
        Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest);
        Task<NodeSummary?> GetNodeSummaryAsync(int id);
        Task<int> CountAsync(SearchRequest searchRequest);
    }
}
