using Microsoft.Extensions.DependencyInjection;

namespace OtherSideCore.WebApi.Interfaces
{
   public interface IWebApiModuleBootstrapper
   {
      void RegisterControllers(IMvcBuilder mvcBuilder);
   }
}
