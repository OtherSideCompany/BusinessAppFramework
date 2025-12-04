using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
    public interface IDomainObjectService<T> where T : DomainObject, new()
    {
        Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

        Task<(bool Success, List<T> Items)> TryGetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default);

        Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);

        Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default);

        Task CreateAsync(T domainObject, DomainObject? parent);

        Task<T> CreateAsync(DomainObject? parent);

        Task SaveAsync(T domainObject);

        Task SaveIndexAsync(IIndexable domainObject);

        Task<bool> DeleteAsync(int domainObjectId);

        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken = default);

        Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken = default);

        Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default);

        Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken = default);

        Task<int?> GetParentIdAsync<U>(int childDomainObjectId, CancellationToken cancellationToken = default) where U : DomainObject;
    }
}
