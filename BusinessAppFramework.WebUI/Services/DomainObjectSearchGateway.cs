using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectSearchGateway<TSearchResult> : HttpService, ISearchGateway<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        private string _controllerKey => _searchRouteKeyRegistry.GetRouteKey<TSearchResult>();
        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Search}/{_controllerKey}";

        private ISearchRouteKeyRegistry _searchRouteKeyRegistry;

        #endregion

        #region Constructor

        public DomainObjectSearchGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            ISearchRouteKeyRegistry searchRouteKeyRegistry)
            : base(clientFactory, apiClientOptions)
        {
            _searchRouteKeyRegistry = searchRouteKeyRegistry;
        }

        #endregion

        #region Public Methods

        public async Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest)
        {
            var route = $"{_baseUrl}/{SearchRouteSegments.Paginated}";
            return await SendSearchAsync(route, paginatedSearchRequest);
        }

        public async Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest)
        {
            var route = $"{_baseUrl}";
            return await SendSearchAsync(route, searchRequest);
        }

        public async Task<TSearchResult?> GetSearchResultAsync(int domainObjectId)
        {
            var route = $"{_baseUrl}/{domainObjectId}";
            return (await GetAsync<TSearchResult>(route)).Data;
        }

        public async Task<NodeSummary?> GetNodeSummaryAsync(int domainObjectId)
        {
            var route = $"{_baseUrl}/{SearchRouteSegments.Summary}/{domainObjectId}";
            return (await GetAsync<NodeSummary>(route)).Data;
        }

        #endregion

        #region Private Methods

        private async Task<SearchResult<TSearchResult>> SendSearchAsync<TPayload>(string searchApiRoute, TPayload payload)
        {
            return (await PostAsync<SearchResult<TSearchResult>>(searchApiRoute, payload)).Data ?? new SearchResult<TSearchResult>();
        }

        #endregion
    }
}
