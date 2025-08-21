using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Interfaces
{
   public interface IModule
   {
      void RegisterServices(IServiceCollection services);
      void RegisterRepositories(IServiceProvider serviceProvider);
      void RegisterDomainObjectServices(IServiceProvider serviceProvider);
      StringKey? GetModuleWorkspaceKey();
      List<StringKey> GetWorkspacesKeys();
   }
}
