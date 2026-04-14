using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDomainObjectServiceGateway<T> where T : DomainObject, new()
   {
      Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);
      Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default);
      Task<DomainObjectReference?> GetHydratedDomainObjectReference(int domainObjectReference, string key);
      Task<DomainObjectReferenceListItem?> GetHydratedDomainObjectReferenceListItem(int domainObjectReferenceListItemId, string key);
      Task<T?> CreateAsync();
      Task SaveAsync(T domainObject, CancellationToken cancellationToken = default);
      Task DeleteAsync(int domainObjectId);
   }
}
