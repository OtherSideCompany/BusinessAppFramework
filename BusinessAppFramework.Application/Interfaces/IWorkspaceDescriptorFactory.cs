using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IWorkspaceDescriptorFactory
    {
        void RegisterWorkspaceDescriptor(string key, Func<WorkspaceDescriptor> workspaceDescriptorFactory);
        void DecorateWorkspaceDescriptor(string key, Action<WorkspaceDescriptor> decorator);
        WorkspaceDescriptor GetWorkspaceDescriptor(string key);
    }
}
