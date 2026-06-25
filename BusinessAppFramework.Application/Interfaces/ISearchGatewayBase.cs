using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchGatewayBase
    {
        Task<DomainObjectSearchResult?> GetDomainObjectSearchResultAsync(int domainObjectId);
        Task<List<DomainObjectSearchResult>> GetDomainObjectSearchResultsAsync(List<int> domainObjectIds);
        Task<SearchResult<DomainObjectSearchResult>> SearchDomainObjectsAsync(SearchRequest searchRequest);
        Task<SearchResult<DomainObjectSearchResult>> PaginatedSearchDomainObjectsAsync(PaginatedSearchRequest paginatedSearchRequest);
        Task<NodeSummary?> GetNodeSummaryAsync(int id);
        Task<int> CountAsync(SearchRequest searchRequest);
    }
}
