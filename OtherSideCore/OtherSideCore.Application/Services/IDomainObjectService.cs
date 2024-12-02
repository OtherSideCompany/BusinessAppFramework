using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectService<T> where T : DomainObject, new()
   {
      Task<T> GetAsync(int entityId, CancellationToken cancellationToken = default);

      Task CreateAsync(T domainObject, DomainObject? parent);

      Task<T> CreateAsync(DomainObject? parent);

      Task SaveAsync(T domainObject);

      Task DeleteAsync(T domainObject);
   }
}
