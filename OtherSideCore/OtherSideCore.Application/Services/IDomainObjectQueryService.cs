using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Application.Services
{
    public interface IDomainObjectQueryService<T> : IDisposable where T : DomainObject, new()
   {
      Task<List<T>> GetAll(CancellationToken cancellationToken = default);

      Task<List<T>> SearchAsync(List<string> filters, Constraint<T> constraint, bool extendedSearch = false, CancellationToken cancellationToken = default);

      Task<PagedResult<T>> PaginatedSearchAsync(List<string> filters, Constraint<T> constraint, bool extendedSearch = false, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
   }
}
