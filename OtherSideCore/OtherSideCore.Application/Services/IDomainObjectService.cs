using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectService<T> where T : DomainObject, new()
   {
      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken = default);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task CreateAsync(T domainObject, DomainObject? parent);

      Task<T> CreateAsync(DomainObject? parent);

      Task SaveAsync(T domainObject);

      Task SaveIndexAsync(IIndexable domainObject);

      Task DeleteAsync(T domainObject);

      Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<DomainObjectReference> CreateDomainObjectReferenceAsync(int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken = default);

      Task DeleteDomainObjectReferenceAsync(int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default);
   }
}
