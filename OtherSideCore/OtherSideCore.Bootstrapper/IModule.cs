using Microsoft.Extensions.DependencyInjection;

namespace OtherSideCore.Bootstrapper
{
   public interface IModule
   {
      void RegisterServices(IServiceCollection services);
   }
}
