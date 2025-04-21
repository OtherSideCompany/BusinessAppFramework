using OtherSideCore.Application.Browser;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Search
{
   public interface IDomainObjectSearch<T> : IDisposable where T : DomainObject, new()
   {
      void ClearActivableConstraints();
      List<Constraint<T>> GetActivableConstraints();
      void SetActivableConstraints(List<Constraint<T>> constraints);
      void ActivateConstraint(Constraint<T> constraint);
      Task SearchAsync(bool extendedSearch, List<string> filters, DomainObject parent = null);
      Task<DomainObjectSearchResult> GetSearchResultAsync(int domainObjectId);
      Task PaginatedSearchAsync(bool resetPages, bool extendedSearch, List<string> filters, DomainObject parent = null);
      Task AddSearchResultAsync(int domainObjectId);
      void RemoveSearchResult(int domainObjectId);
      void Dispose();
   }
}
