using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using PuppeteerSharp.Input;

namespace BusinessAppFramework.Application.Factories
{
    public class WorkspaceDescriptorFactory : stringBasedFactory, IWorkspaceDescriptorFactory
    {
        public WorkspaceDescriptor GetWorkspaceDescriptor(string key)
        {
            return (WorkspaceDescriptor)Create(key);
        }

        public void RegisterWorkspaceDescriptor(string key, Func<WorkspaceDescriptor> workspaceDescriptorFactory)
        {
            Register(key, workspaceDescriptorFactory);
        }
    }
}


