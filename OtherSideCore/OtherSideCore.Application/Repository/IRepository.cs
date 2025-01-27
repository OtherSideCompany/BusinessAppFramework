using OtherSideCore.Application.Search;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Repository
{
   public interface IRepository<T> : IDisposable where T : DomainObject, new()
   {
      Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectSearchResult>> SearchAsync(List<string> filters, bool extendedSearch, Expression<Func<T, bool>> where, DomainObject? parent, CancellationToken cancellationToken);

      Task<DomainObjectSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken);

      Task<List<DomainObjectSearchResult>> PaginatedSearchAsync(List<string> filters, bool extendedSearch, Expression<Func<T, bool>> where, DomainObject? parent, int pageNumber, int pageSize, CancellationToken cancellationToken);

      Task CreateAsync(T domainObject, DomainObject? parent, int userId, string userName);

      Task SaveAsync(T domainObject, int userId, string userName);

      Task SaveIndexAsync(IIndexable domainObject, int userId, string userName);

      Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken);

      Task DeleteAsync(T domainObject);

      Task<DateTime> GetLastModificatonTimeAsync(T domainObject, CancellationToken cancellationToken);

      Task<int> CountAsync(List<string> filters, bool extendedSearch, Expression<Func<T, bool>> predicate, DomainObject? parent, CancellationToken cancellationToken);

      Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken);

      Task<List<DomainObjectReference>> GetDomainObjectReferences(int domainObjectId, CancellationToken cancellationToken);

      Task<DomainObjectReference> CreateDomainObjectReferenceAsync(int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken);

      Task DeleteDomainObjectReferenceAsync(int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken);
   }
}
