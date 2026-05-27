using BusinessAppFramework.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IModuleBootstrapper
    {
        void RegisterBackendServices(IServiceCollection services);
        void RegisterFrontendServices(IServiceCollection services);
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
        void RegisterComponents(IServiceProvider serviceProvider);
        void RegisterTrees(IServiceProvider serviceProvider);
        void RegisterReferences(IServiceProvider serviceProvider);
        void RegisterParentChildRelations(IServiceProvider serviceProvider);
        StringKey? GetModuleWorkspaceKey();
        List<StringKey> GetWorkspacesKeys();
    }
}
