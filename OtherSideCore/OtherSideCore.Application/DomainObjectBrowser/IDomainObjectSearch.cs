using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public interface IDomainObjectSearch<T> : IDisposable where T : DomainObject, new()
   {
      void ClearFilters();
      void ClearConstraints();
      List<Constraint<T>> GetConstraints();
      void SetConstraints(List<Constraint<T>> constraints);
      void ActivateConstraint(Constraint<T> constraint);
      Task SearchAsync(CancellationToken cancellationToken);
      Task PaginatedSearchAsync(bool resetPages, CancellationToken cancellationToken);
      void AddSearchResult(DomainObject domainObject);
      void RemoveSearchResult(DomainObject domainObject);      
      void Dispose();
   }
}
