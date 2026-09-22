using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Components.Pages.Settings
{
    public static class SettingsWorkspaceRegistration
    {
        public static void RegisterSettingsWorkspace(this IServiceProvider serviceProvider)
        {
            var workspaceDescriptorFactory = serviceProvider.GetRequiredService<IWorkspaceDescriptorFactory>();
            var iconFactory = serviceProvider.GetRequiredService<IIconFactory>();

            workspaceDescriptorFactory.RegisterWorkspaceDescriptor(
                SettingsKeys.Workspace,
                () => new WorkspaceDescriptor
                {
                    WorkspaceKey = SettingsKeys.Workspace,
                    ComponentType = typeof(SettingsPage)
                });

            iconFactory.RegisterIcon(SettingsKeys.Workspace, Icons.Material.Outlined.Settings);
            iconFactory.RegisterIcon(SettingsKeys.SettingsKey, Icons.Material.Outlined.Settings);
        }
    }
}
