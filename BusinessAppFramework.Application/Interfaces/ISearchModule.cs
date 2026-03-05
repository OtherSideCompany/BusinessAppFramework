using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchModule
    {
        void RegisterServices(IServiceCollection services);
        void RegisterSearchRouteKeys(IServiceProvider serviceProvider);

    }
}
