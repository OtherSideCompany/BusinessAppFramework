using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.Application.Factories
{
    public class WorkspaceDescriptorFactory : StringBasedFactory, IWorkspaceDescriptorFactory
    {
        public WorkspaceDescriptor GetWorkspaceDescriptor(string key)
        {
            return (WorkspaceDescriptor)Create(key);
        }

        public void RegisterWorkspaceDescriptor(string key, Func<WorkspaceDescriptor> workspaceDescriptorFactory)
        {
            Register(key, workspaceDescriptorFactory);
        }

        public void DecorateWorkspaceDescriptor(string key, Action<WorkspaceDescriptor> decorator)
        {
            Decorate(key, workspaceDescriptor =>
            {
                decorator((WorkspaceDescriptor)workspaceDescriptor);
                return workspaceDescriptor;
            });
        }
    }
}


