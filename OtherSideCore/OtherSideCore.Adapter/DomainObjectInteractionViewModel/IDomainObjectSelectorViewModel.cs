using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSelectorViewModel : IDisposable
   {
      event EventHandler SelectionValidated;
      Selection Selection { get; }
      Task InitializeAsync();
      bool CanValidateSelection();
      void ValidateSelection();
   }
}
