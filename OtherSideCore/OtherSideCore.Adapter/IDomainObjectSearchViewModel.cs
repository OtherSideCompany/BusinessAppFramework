using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter
{
   public interface IDomainObjectSearchViewModel
   {
      bool CanSelectSearchResult(DomainObjectViewModel domainObjectViewModel);
      Task SelectSearchResultAsync(DomainObjectViewModel domainObjectViewModel, CancellationToken cancellationToken = default);
   }
}
