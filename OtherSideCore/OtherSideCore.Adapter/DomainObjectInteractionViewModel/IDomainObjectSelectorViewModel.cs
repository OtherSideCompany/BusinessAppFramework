using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSelectorViewModel : IDisposable, IDomainObjectBrowserViewModel
   {
      event EventHandler SelectionValidated;
      Selection Selection { get; }
      bool DynamicSearch { get; set; }
      Task InitializeAsync();
      bool CanValidateSelection();
      void ValidateSelection();
      Task<DomainObject> GetSelectedSearchResultDomainObjectAsync();
   }
}
