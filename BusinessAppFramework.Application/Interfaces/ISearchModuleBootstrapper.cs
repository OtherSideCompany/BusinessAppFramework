using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchModuleBootstrapper
    {
        IEnumerable<Assembly> GetBackendServiceAssemblies();
        IEnumerable<Assembly> GetFrontendServiceAssemblies();
        void RegisterBackendServices(IServiceCollection services);
        void RegisterFrontendServices(IServiceCollection services);
        void RegisterSearchRouteKeys(IServiceProvider serviceProvider);

    }
}
