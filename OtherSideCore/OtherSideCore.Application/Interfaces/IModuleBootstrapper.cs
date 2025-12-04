using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Interfaces
{
    public interface IModuleBootstrapper
    {
        void RegisterServices(IServiceCollection services);   
        void RegisterLocalizedStrings(IServiceProvider serviceProvider);
        void RegisterBrowserDescriptors(IServiceProvider serviceProvider);
        void RegisterConstraints(IServiceProvider serviceProvider);
        void RegisterIcons(IServiceProvider serviceProvider);
        StringKey? GetModuleWorkspaceKey();
        List<StringKey> GetWorkspacesKeys();
    }
}
