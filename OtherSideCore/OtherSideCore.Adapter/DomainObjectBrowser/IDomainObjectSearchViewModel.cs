using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public interface IDomainObjectSearchViewModel : IDisposable
   {
      Task SearchAsync(CancellationToken cancellationToken = default);
      Task PaginatedSearchAsync(bool resetPage, CancellationToken cancellationToken = default);
      void LoadSearchResultViewModels();
      void UnloadSearchResultViewModels();
      void AddSearchResultViewModel(DomainObjectViewModel domainObjectViewModel);
      void RemoveSearchResultViewModel(DomainObjectViewModel domainObjectViewModel);
   }
}
