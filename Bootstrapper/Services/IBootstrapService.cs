using Microsoft.Extensions.DependencyInjection;

namespace Bootstrapper.Services
{
   public interface IBootstrapService
   {
      IServiceCollection GetServices();
   }
}
