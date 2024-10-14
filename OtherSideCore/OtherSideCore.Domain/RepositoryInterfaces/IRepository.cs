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
      Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

      Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);

      Task<List<T>> GetAllPaginatedAsync(Expression<Func<T, bool>> where, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

      Task CreateAsync(T domainObject, int userId);

      Task SaveAsync(T domainObject, int userId);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task LoadAsync(T domainObject);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject);

      Task LoadTrackingInfos(T domainObject);

      Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
   }
}
