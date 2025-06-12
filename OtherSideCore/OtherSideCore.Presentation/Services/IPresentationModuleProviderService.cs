using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Presentation.Services
{
   public interface IPresentationModuleProviderService
   {
      List<IPresentationModule> GetPresentationModules();
   }
}
