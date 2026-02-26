using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Services
{
   public interface IDomainObjectService<T> where T : DomainObject, new()
   {
      Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);
      Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

      Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<DomainObjectReference> GetHydratedDomainObjectReference(int domainObjectId, string relationKey);
      Task<DomainObjectReferenceListItem> GetHydratedDomainObjectReferenceListItem(int domainObjectId, string relationKey);

      Task<T?> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default);

      Task CreateAsync(T domainObject);

      Task<T> CreateAsync();

      Task SaveAsync(T domainObject);

      Task SaveIndexAsync(IIndexable domainObject);

      Task<bool> DeleteAsync(int domainObjectId);
   }
}
