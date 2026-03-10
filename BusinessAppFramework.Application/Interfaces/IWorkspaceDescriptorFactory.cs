using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IWorkspaceDescriptorFactory
    {
        void RegisterWorkspaceDescriptor(StringKey key, Func<WorkspaceDescriptor> workspaceDescriptorFactory);
        WorkspaceDescriptor GetWorkspaceDescriptor(StringKey key);
    }
}
