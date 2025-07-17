using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
   public interface IRepository<T> : IDisposable where T : DomainObject
   {
      Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task CreateAsync(T domainObject, DomainObject? parent);

      Task SaveAsync(T domainObject);

      Task SaveIndexAsync(IIndexable domainObject);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);      

      Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken);

      Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken);

      Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken);

      Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken);

      Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject;
   }
}
