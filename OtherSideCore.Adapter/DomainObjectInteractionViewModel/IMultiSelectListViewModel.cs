using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public interface IMultiSelectListViewModel : IDisposable, ISavable
   {
      Task InitializeAsync(DomainObjectViewModel contextViewModel);
   }
}
