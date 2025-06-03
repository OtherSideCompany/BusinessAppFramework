using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Bootstrapper
{
   public interface IModule
   {
      void RegisterServices(IServiceCollection services);
      void RegisterRepositories(IRepositoryFactory repositoryFactory);
   }
}
