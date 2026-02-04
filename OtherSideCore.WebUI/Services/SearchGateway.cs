using Microsoft.Extensions.Options;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Trees;
using OtherSideCore.Contracts;
using OtherSideCore.WebUI.Interfaces;
using OtherSideCore.WebUI.Services;

namespace Manufacturing.WebUI.Services
{
    public class SearchGateway<TSearchResult> : HttpService, ISearchGateway<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        

        #endregion

        #region Constructor

        public SearchGateway(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
            : base(clientFactory, apiClientOptions)
        {
            
        }

        #endregion

        #region Public Methods

        public async Task<SearchResult<TSearchResult>> PaginatedSearchAsync(PaginatedSearchRequest paginatedSearchRequest)
        {
            return await SendSearchAsync(Routes.BuildRoute(Routes.PaginatedSearchTemplate, typeof(TSearchResult), null), paginatedSearchRequest);
        }

        public async Task<SearchResult<TSearchResult>> SearchAsync(SearchRequest searchRequest)
        {
            return await SendSearchAsync(Routes.BuildRoute(Routes.SearchTemplate, typeof(TSearchResult), null), searchRequest);
        }

        public async Task<TSearchResult?> GetSearchResultAsync(int domainObjectId)
        {
            return (await GetAsync<TSearchResult>(Routes.BuildRoute(Routes.SpecificSearchTemplate, typeof(TSearchResult), domainObjectId))).Data;
        }

        public async Task<NodeSummary?> GetNodeSummaryAsync(int id)
        {
            return (await GetAsync<NodeSummary>(Routes.BuildRoute(Routes.GetSummaryTemplate, typeof(TSearchResult), id))).Data;
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
