using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchModule
    {
        void RegisterBackendServices(IServiceCollection services);
        void RegisterFrontendServices(IServiceCollection services);
        void RegisterSearchRouteKeys(IServiceProvider serviceProvider);

    }
}
