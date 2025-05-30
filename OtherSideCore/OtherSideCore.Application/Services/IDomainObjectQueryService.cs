using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectQueryService<TSearchResult> : IDisposable where TSearchResult : DomainObjectSearchResult, new()
   {
      Task<List<TSearchResult>> SearchAsync(List<string> filters, Constraint<TSearchResult> constraint, bool extendedSearch = false, CancellationToken cancellationToken = default);

      Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken = default);

      Task<PagedResult<TSearchResult>> PaginatedSearchAsync(List<string> filters, Constraint<TSearchResult> constraint, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
   }
}
