using OtherSideCore.Application.Trees;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IDomainObjectServiceGateway<T> where T : DomainObject, new()
    {
        //Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

        //Task<(bool Success, List<T> Items)> TryGetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default);

        //Task<List<T>> GetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default);

        Task<List<int>> GetChildrenIdsAsync(int parentId, string key, CancellationToken cancellationToken = default);

        Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task<DomainObjectReference?> GetHydratedDomainObjectReference(int domainObjectReference, string key);
        Task<DomainObjectReferenceListItem?> GetHydratedDomainObjectReferenceListItem(int domainObjectReferenceListItemId, string key);

        //Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default);

        //Task CreateAsync(T domainObject, DomainObject? parent);

        Task<T?> CreateAsync();

        Task SaveAsync(T domainObject, CancellationToken cancellationToken = default);

        //Task SaveIndexAsync(IIndexable domainObject);

        Task DeleteAsync(int domainObjectId);

        //Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey key, int domainObjectId, CancellationToken cancellationToken = default);

        //Task CreateDomainObjectReferenceAsync(StringKey key, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken = default);

        //Task DeleteDomainObjectReferenceAsync(StringKey key, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default);

        //Task SetParentAsync(int parentId, int childId, string key, CancellationToken cancellationToken = default);

        //Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject;
    }
}
