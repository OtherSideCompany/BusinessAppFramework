using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Application.Interfaces
{
    public interface ISearchModule
    {
        void RegisterServices(IServiceCollection services);
    }
}
