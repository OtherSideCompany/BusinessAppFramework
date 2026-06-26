using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IDomainObjectServiceGateway<T> where T : DomainObject, new()
    {
        Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(List<int> domainObjectIds, CancellationToken cancellationToken = default);
        Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllHydratedAsync(List<int> domainObjectIds, CancellationToken cancellationToken = default);        
        Task<T?> CreateAsync();
        Task<DomainObjectApplicationActionResultPayload> CreateAsync(T domainobject);
        Task<DomainObjectApplicationActionResultPayload> SaveAsync(T domainObject, CancellationToken cancellationToken = default);
        Task DeleteAsync(int domainObjectId);
    }
}
