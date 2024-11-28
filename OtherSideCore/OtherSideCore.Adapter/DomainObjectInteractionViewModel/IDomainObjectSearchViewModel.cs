using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSearchViewModel : IDisposable
   {
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void LoadSearchResultViewModels();
      void UnloadSearchResultViewModels();
      void AddSearchResultViewModel(DomainObjectViewModel domainObjectViewModel);
      void RemoveSearchResultViewModel(DomainObjectViewModel domainObjectViewModel);
   }
}
