using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectQueryService<T> : IDisposable where T : DomainObject, new()
   {
      Task<List<DomainObjectSearchResult>> SearchAsync(List<string> filters, Constraint<T> constraint, DomainObject? parent, bool extendedSearch = false, CancellationToken cancellationToken = default);

      Task<DomainObjectSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<PagedResult<T>> PaginatedSearchAsync(List<string> filters, Constraint<T> constraint, DomainObject? parent, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
   }
}
