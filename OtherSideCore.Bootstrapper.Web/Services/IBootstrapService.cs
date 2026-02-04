using Microsoft.Extensions.DependencyInjection;

namespace OtherSideCore.Bootstrapper.Web.Services
{
   public interface IBootstrapService
   {
      IServiceCollection GetServices();
   }
}
