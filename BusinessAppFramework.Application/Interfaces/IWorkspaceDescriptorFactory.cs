using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IWorkspaceDescriptorFactory
    {
        void RegisterWorkspaceDescriptor(string key, Func<WorkspaceDescriptor> workspaceDescriptorFactory);
        WorkspaceDescriptor GetWorkspaceDescriptor(string key);
    }
}
