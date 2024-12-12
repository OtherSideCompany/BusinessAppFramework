using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Repository
{
   public interface IRepository<T> : IDisposable where T : DomainObject, new()
   {
      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectSearchResult>> SearchAsync(Expression<Func<T, bool>> where, DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectSearchResult>> PaginatedSearchAsync(Expression<Func<T, bool>> where, DomainObject? parent, int pageNumber, int pageSize, CancellationToken cancellationToken);

      Task CreateAsync(T domainObject, DomainObject? parent, int userId);

      Task SaveAsync(T domainObject, int? userId);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);

      Task<int> CountAsync(Expression<Func<T, bool>> predicate, DomainObject? parent, CancellationToken cancellationToken);
      Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);
   }
}
