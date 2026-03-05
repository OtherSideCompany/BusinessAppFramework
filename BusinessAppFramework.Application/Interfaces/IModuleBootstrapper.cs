using BusinessAppFramework.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IModuleBootstrapper
    {
        void RegisterServices(IServiceCollection services);
        void RegisterLocalizedStrings(IServiceProvider serviceProvider);
        void RegisterBrowserDescriptors(IServiceProvider serviceProvider);
        void RegisterSelectorDescriptors(IServiceProvider serviceProvider);
        void RegisterReferenceSelectors(IServiceProvider serviceProvider);
        void RegisterDomainObjectBrowserWorkspaces(IServiceProvider serviceProvider);
        void RegisterConstraints(IServiceProvider serviceProvider);
        void RegisterIcons(IServiceProvider serviceProvider);
        void RegisterDomainObjectTypesMapping(IServiceProvider serviceProvider);
        void RegisterWorkflows(IServiceProvider serviceProvider);
        void RegisterWorkflowContextLoader(IServiceProvider serviceProvider);
        void RegisterDomainObjectRouteKeys(IServiceProvider serviceProvider);
        StringKey? GetModuleWorkspaceKey();
        List<StringKey> GetWorkspacesKeys();
    }
}
