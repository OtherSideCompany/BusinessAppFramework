using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectService<T> where T : DomainObject, new()
   {
      Task<T> GetAsync(int entityId, CancellationToken cancellationToken = default);

      Task LoadAsync(T domainObject);

      Task CreateAsync(T domainObject);

      Task SaveAsync(T domainObject);

      Task DeleteAsync(T domainObject);

      Task LoadTrackingInfosAsync(T domainObject);
   }
}
