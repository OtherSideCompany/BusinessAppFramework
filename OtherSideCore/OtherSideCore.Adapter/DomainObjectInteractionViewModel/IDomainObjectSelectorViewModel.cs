using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSelectorViewModel
   {
      bool CanValidateSelection();
      void ValidateSelection();
   }
}
