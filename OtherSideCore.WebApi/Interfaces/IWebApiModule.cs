using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.WebApi.Interfaces
{
   public interface IWebApiModule
   {
      void RegisterControllers(IMvcBuilder mvcBuilder);
   }
}
