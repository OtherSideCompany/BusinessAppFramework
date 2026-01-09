using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
    public interface IRepository<T> : IDisposable where T : DomainObject
    {
        Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);        

        Task CreateAsync(T domainObject, DomainObject? parent);

        Task SaveAsync(T domainObject);

        Task SaveIndexAsync(IIndexable domainObject);

        Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

        Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken);

        Task DeleteAsync(int domainObjectId);

        Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);     

        Task<bool> IsSystemObjectAsync(int domainObjectId);
    }
}
