using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BusinessAppFramework.Bootstrapper.Services
{
   public interface IBootstrapService
   {
      IServiceCollection GetServices(ConfigurationManager configurationManager);
   }
}
