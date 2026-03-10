using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using PuppeteerSharp.Input;

namespace BusinessAppFramework.Application.Factories
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


