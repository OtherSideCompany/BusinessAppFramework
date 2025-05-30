using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
   public interface IRepository<T> : IDisposable where T : DomainObject
   {
      Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);    

      Task CreateAsync(T domainObject, DomainObject? parent, int userId, string userName);

      Task SaveAsync(T domainObject, int userId, string userName);

      Task SaveIndexAsync(IIndexable domainObject, int userId, string userName);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);      

      Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId, CancellationToken cancellationToken);

      Task<DomainObjectReference> CreateDomainObjectReferenceAsync(int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken);

      Task DeleteDomainObjectReferenceAsync(int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken);

      Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken);

      Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject;
   }
}
