using OtherSideCore.Application.Descriptors;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;

namespace OtherSideCore.WebUI.Factories
{
    public class WorkspaceDescriptorFactory : StringKeyBasedFactory, IWorkspaceDescriptorFactory
    {
        public WorkspaceDescriptor GetWorkspaceDescriptor(StringKey key)
        {
            return (WorkspaceDescriptor)Create(key);
        }

        public void RegisterWorkspaceDescriptor(StringKey key, Func<WorkspaceDescriptor> workspaceDescriptorFactory)
        {
            Register(key, workspaceDescriptorFactory);
        }        
    }
}
