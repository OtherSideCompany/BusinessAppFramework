using OtherSideCore.Application.Browser;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Search
{
   public interface IDomainObjectSearch<TSearchResult> : IDisposable where TSearchResult : DomainObjectSearchResult, new()
   {
      void ClearActivableConstraints();
      List<Constraint<TSearchResult>> GetActivableConstraints();
      void SetActivableConstraints(List<Constraint<TSearchResult>> constraints);
      void ActivateConstraint(Constraint<TSearchResult> constraint);
      Task SearchAsync(bool extendedSearch, List<string> filters);
      Task<DomainObjectSearchResult> GetSearchResultAsync(int domainObjectId);
      Task PaginatedSearchAsync(bool resetPages, bool extendedSearch, List<string> filters);
      Task AddSearchResultAsync(int domainObjectId);
      void RemoveSearchResult(int domainObjectId);
      void Dispose();
   }
}
