using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Services
{
   public interface ISearchService<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      Task<int> CountAsync(List<string> filters, bool extendedSearch, Expression<Func<TSearchResult, bool>> predicate, CancellationToken cancellationToken);
      Task<List<TSearchResult>> SearchAsync(List<string> filters, bool extendedSearch, Expression<Func<TSearchResult, bool>> where, CancellationToken cancellationToken);
      Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken);
      Task<List<TSearchResult>> PaginatedSearchAsync(List<string> filters, bool extendedSearch, Expression<Func<TSearchResult, bool>> where, int pageNumber, int pageSize, CancellationToken cancellationToken);
   }
}
