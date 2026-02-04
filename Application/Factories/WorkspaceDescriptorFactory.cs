using Application.Descriptors;
using Application.Interfaces;
using Domain;

namespace Application.Factories
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
