using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectQueryService<TSearchResult> : IDomainObjectQueryService<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields

      protected ISearchServiceFactory _searchServiceFactory;
      protected ISearchService<TSearchResult> _searchService;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectQueryService(ISearchServiceFactory searchServiceFactory)
      {
         _searchServiceFactory = searchServiceFactory;
         _searchService = (ISearchService<TSearchResult>)_searchServiceFactory.CreateSearchService<TSearchResult>();
      }

      #endregion

      #region Public Methods

      public virtual async Task<List<TSearchResult>> SearchAsync(List<string> filters, Constraint<TSearchResult> constraint, bool extendedSearch = false, CancellationToken cancellationToken = default)
      {
         var constraintExpression = constraint == null ? Constraint<TSearchResult>.Empty.Expression : constraint.Expression;         

         var totalCount = await _searchService.CountAsync(filters, extendedSearch, constraintExpression, cancellationToken);
         return await _searchService.SearchAsync(filters, extendedSearch, constraintExpression, cancellationToken);
      }

      public virtual async Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return (TSearchResult)await _searchService.SearchAsync(domainObjectId, cancellationToken);
      }

      public virtual async Task<PagedResult<TSearchResult>> PaginatedSearchAsync(List<string> filters, Constraint<TSearchResult> constraint, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
      {
         var constraintExpression = constraint == null ? Constraint<TSearchResult>.Empty.Expression : constraint.Expression;

         var totalCount = await _searchService.CountAsync(filters, extendedSearch, constraintExpression, cancellationToken);
         var results = await _searchService.PaginatedSearchAsync(filters, extendedSearch, constraintExpression, pageNumber, pageSize, cancellationToken);

         return new PagedResult<TSearchResult>
         {
            Items = results,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
         };
      }

      public void Dispose()
      {

      }
      
      #endregion

      #region Private Methods



      #endregion
   }
}
