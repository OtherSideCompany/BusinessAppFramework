using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Bootstrapper.Services
{
   public interface IBootstrapService
   {
      IServiceCollection GetServices();
   }
}
