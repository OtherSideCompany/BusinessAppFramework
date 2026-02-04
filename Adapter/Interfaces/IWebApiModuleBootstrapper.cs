using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Interfaces
{
   public interface IWebApiModuleBootstrapper
   {
      void RegisterControllers(IMvcBuilder mvcBuilder);
   }
}
