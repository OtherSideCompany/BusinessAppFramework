using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public interface IDomainObjectSelectorViewModel
   {
      bool CanValidateSelection();
      void ValidateSelection();
   }
}
