using Application.Descriptors;
using Domain;

namespace Application.Interfaces
{
   public interface IWorkspaceDescriptorFactory
   {
      void RegisterWorkspaceDescriptor(StringKey key, Func<WorkspaceDescriptor> workspaceDescriptorFactory);
      WorkspaceDescriptor GetWorkspaceDescriptor(StringKey key);
   }
}
