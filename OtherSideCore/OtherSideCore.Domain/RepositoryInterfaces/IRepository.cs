using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.RepositoryInterfaces
{
   public interface IRepository<T> : IDisposable where T : DomainObject
   {
      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, DomainObject? parent, CancellationToken cancellationToken);

      Task<List<T>> GetAllPaginatedAsync(Expression<Func<T, bool>> where, DomainObject? parent, int pageNumber, int pageSize, CancellationToken cancellationToken);

      Task CreateAsync(T domainObject, DomainObject? parent, int userId);

      Task SaveAsync(T domainObject, int? userId);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);

      Task<int> CountAsync(Expression<Func<T, bool>> predicate, DomainObject? parent, CancellationToken cancellationToken);
      Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);
   }
}
