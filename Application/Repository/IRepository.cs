using Domain;
using Domain.DomainObjects;

namespace Application.Repository
{
   public interface IRepository<T> : IDisposable where T : DomainObject
   {
      Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task CreateAsync(T domainObject);

      Task SaveAsync(T domainObject);

      Task SaveIndexAsync(IIndexable domainObject);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken);

      Task DeleteAsync(int domainObjectId);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);

      Task<bool> IsSystemObjectAsync(int domainObjectId);
   }
}
