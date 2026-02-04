using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherSideCore.Application;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Application.Trees;
using OtherSideCore.Contracts;

namespace OtherSideCore.Adapter.Web.Controllers
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

        [HttpPost(Routes.CountTemplate)]
        public virtual async Task<ActionResult<int>> Count([FromBody] SearchRequest searchRequest)
        {
            var result = await _searchService.CountAsync(searchRequest);
            return Ok(result);
        }

        [HttpPost(Routes.SearchTemplate)]
        public virtual async Task<ActionResult<SearchResult<TSearchResult>>> Search([FromBody] SearchRequest searchRequest)
        {
            var result = await _searchService.SearchAsync(searchRequest);
            return Ok(result);
        }

        [HttpPost(Routes.PaginatedSearchTemplate)]
        public async Task<ActionResult<SearchResult<TSearchResult>>> PaginatedSearch([FromBody] PaginatedSearchRequest paginatedSearchRequest)
        {
            var result = await _searchService.PaginatedSearchAsync(paginatedSearchRequest);
            return Ok(result);
        }

        [HttpPost(Routes.SpecificSearchTemplate)]
        public async Task<ActionResult<TSearchResult>> GetSearchResultAsync([FromBody] int domainObjectId)
        {
            var result = await _searchService.SearchAsync(domainObjectId);
            return Ok(result);
        }

        [HttpGet(Routes.GetSummaryTemplate)]
        public async Task<ActionResult<NodeSummary?>> GetSummaryAsync(int domainObjectId)
        {
            var result = await _searchService.GetSummaryAsync(domainObjectId);
            return Ok(result);
        }

        #endregion
    }
}
