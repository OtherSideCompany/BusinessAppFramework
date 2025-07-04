using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtherSideCore.Adapter.Services;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
    public interface IDomainObjectInteractionHost
   {
      IDomainObjectInteractionService DomainObjectInteractionService { get; }
   }
}
