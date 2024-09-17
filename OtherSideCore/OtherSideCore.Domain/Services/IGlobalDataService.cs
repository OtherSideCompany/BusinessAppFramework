using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Services
{
   public interface IGlobalDataService
   {
      Task LoadGlobalDataAsync();
   }
}
