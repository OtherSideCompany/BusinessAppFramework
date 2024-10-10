using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

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

      public async Task<List<T>> GetAll(CancellationToken cancellationToken = default)
      {
         return await _repository.GetAllAsync(cancellationToken);
      }

      public virtual async Task<PagedResult<T>> PaginatedSearchAsync(List<string> filters, Constraint<T> constraint, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
      {
         var constraintExpression = constraint == null ? Constraint<T>.Empty.Expression : constraint.Expression;
         var filterExpressions = GetFilterConstraints(filters, extendedSearch).Select(c => c.Expression);

         var combinedExpressions = constraintExpression;

         foreach (var filterExpression in filterExpressions)
         {
            combinedExpressions = combinedExpressions.And(filterExpression);
         }

         var totalCount = await _repository.CountAsync(combinedExpressions, cancellationToken);

         return new PagedResult<T>
         {
            Items = await _repository.GetAllAsync(combinedExpressions, pageNumber, pageSize, cancellationToken),
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
