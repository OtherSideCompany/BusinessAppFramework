using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
    public interface IRepository<T> : IRelationRepository, IDisposable where T : DomainObject
    {
        Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);

        Task<List<object>> GetChildrenAsync(int parentId, string relationKey, CancellationToken cancellationToken = default);

        Task CreateAsync(T domainObject, DomainObject? parent);

        Task SaveAsync(T domainObject);

        Task SaveIndexAsync(IIndexable domainObject);

        Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

        Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken);

        Task DeleteAsync(int domainObjectId);

        Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);

        Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);


        Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken);

        Task<int?> GetParentIdAsync<U>(int childDomainObjectId, CancellationToken cancellationToken = default) where U : DomainObject;

        Task<bool> IsSystemObjectAsync(int domainObjectId);
    }
}
