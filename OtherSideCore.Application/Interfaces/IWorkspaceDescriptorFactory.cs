using OtherSideCore.Application.Descriptors;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Interfaces
{
    public interface IWorkspaceDescriptorFactory
    {
        void RegisterWorkspaceDescriptor(StringKey key, Func<WorkspaceDescriptor> workspaceDescriptorFactory);
        WorkspaceDescriptor GetWorkspaceDescriptor(StringKey key);
    }
}
