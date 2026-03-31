using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class DomainObjectSearchController<TSearchResult> : ControllerBase
       where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        protected readonly ISearchService<TSearchResult> _searchService;

        #endregion

        #region Constructor

        public DomainObjectSearchController(ISearchService<TSearchResult> searchService)
        {
            _searchService = searchService;
        }

        #endregion

        #region Public Methods

        [HttpPost(SearchRouteSegments.Count)]
        public virtual async Task<ActionResult<int>> Count([FromBody] SearchRequest searchRequest)
        {
            var result = await _searchService.CountAsync(searchRequest);
            return Ok(result);
        }

        [HttpPost()]
        public virtual async Task<ActionResult<SearchResult<TSearchResult>>> Search(
            [FromBody] SearchRequest searchRequest)
        {
            var result = await _searchService.SearchAsync(searchRequest);
            return Ok(result);
        }

        [HttpPost(SearchRouteSegments.Paginated)]
        public async Task<ActionResult<SearchResult<TSearchResult>>> PaginatedSearch(
            [FromBody] PaginatedSearchRequest paginatedSearchRequest)
        {
            var result = await _searchService.PaginatedSearchAsync(paginatedSearchRequest);
            return Ok(result);
        }

        [HttpGet($"{{{ApiRouteParams.DomainObjectId}:int}}")]
        public async Task<ActionResult<TSearchResult>> GetSearchResultAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var result = await _searchService.SearchAsync(domainObjectId);
            return Ok(result);
        }

        [HttpGet($"{SearchRouteSegments.Summary}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public async Task<ActionResult<NodeSummary?>> GetSummaryAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var result = await _searchService.GetSummaryAsync(domainObjectId);
            return Ok(result);
        }

        #endregion
    }
}
