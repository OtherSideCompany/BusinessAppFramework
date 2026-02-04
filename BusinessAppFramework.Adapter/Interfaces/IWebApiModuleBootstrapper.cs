using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Adapter.Interfaces
{
   public interface IWebApiModuleBootstrapper
   {
      void RegisterControllers(IMvcBuilder mvcBuilder);
   }
}
