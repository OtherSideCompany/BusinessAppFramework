using Microsoft.AspNetCore.Mvc;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Adapter.Web.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public abstract class DomainObjectSearchController<TSearchResult> : ControllerBase
      where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields

      protected readonly IDomainObjectSearch<TSearchResult> _searchService;

      #endregion

      #region Constructor

      public DomainObjectSearchController(IDomainObjectSearch<TSearchResult> searchService)
      {
         _searchService = searchService;
      }

      #endregion

      #region Public Methods

      [HttpPost("search")]
      public virtual async Task<ActionResult<List<TSearchResult>>> Search([FromBody] SearchRequest searchRequest)
      {
         await _searchService.SearchAsync(searchRequest.ExtendedSearch, searchRequest.Filters);
         return Ok(_searchService.SearchResults);
      }

      [HttpPost("search/paginated")]
      public async Task<ActionResult<List<TSearchResult>>> PaginatedSearch([FromBody] PaginatedSearchRequest paginatedSearchRequest)
      {
         await _searchService.PaginatedSearchAsync(paginatedSearchRequest.ResetPages, paginatedSearchRequest.ExtendedSearch, paginatedSearchRequest.Filters);
         return Ok(_searchService.SearchResults);
      }

      #endregion
   }
}
