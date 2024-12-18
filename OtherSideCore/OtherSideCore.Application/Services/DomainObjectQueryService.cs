using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Repository;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectQueryService<T> : IDomainObjectQueryService<T> where T : DomainObject, new()
   {
      #region Fields

      protected IRepository<T> _repository;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectQueryService(IRepository<T> repository)
      {
         _repository = repository;
      }

      #endregion

      #region Public Methods

      public virtual async Task<List<DomainObjectSearchResult>> SearchAsync(List<string> filters, Constraint<T> constraint, DomainObject? parent, bool extendedSearch = false, CancellationToken cancellationToken = default)
      {
         var constraintExpression = constraint == null ? Constraint<T>.Empty.Expression : constraint.Expression;

         var totalCount = await _repository.CountAsync(filters, extendedSearch, constraintExpression, parent, cancellationToken);
         return await _repository.SearchAsync(filters, extendedSearch, constraintExpression, parent, cancellationToken);
      }

      public virtual async Task<DomainObjectSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.SearchAsync(domainObjectId, cancellationToken);
      }

      public virtual async Task<PagedResult<T>> PaginatedSearchAsync(List<string> filters, Constraint<T> constraint, DomainObject? parent, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
      {
         var constraintExpression = constraint == null ? Constraint<T>.Empty.Expression : constraint.Expression;

         var totalCount = await _repository.CountAsync(filters, extendedSearch, constraintExpression, parent, cancellationToken);
         var results = await _repository.PaginatedSearchAsync(filters, extendedSearch, constraintExpression, parent, pageNumber, pageSize, cancellationToken);

         return new PagedResult<T>
         {
            Items = results,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
         };
      }

      public virtual List<Constraint<T>> GetFilterConstraints(List<string> filters, bool extendedSearch)
      {
         return new List<Constraint<T>>();
      }

      public void Dispose()
      {

      }
      #endregion

      #region Private Methods



      #endregion
   }
}
