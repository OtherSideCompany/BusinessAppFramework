using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public interface IDomainObjectSearch<T> : IDisposable where T : DomainObject, new()
   {
      void ClearConstraints();
      List<Constraint<T>> GetConstraints();
      void SetConstraints(List<Constraint<T>> constraints);
      void ActivateConstraint(Constraint<T> constraint);
      Task SearchAsync(bool extendedSearch, List<string> filters, DomainObject parent = null);
      Task PaginatedSearchAsync(bool resetPages, bool extendedSearch, List<string> filters, DomainObject parent = null);
      void AddSearchResult(DomainObject domainObject);
      void RemoveSearchResult(DomainObject domainObject);      
      void Dispose();
   }
}
