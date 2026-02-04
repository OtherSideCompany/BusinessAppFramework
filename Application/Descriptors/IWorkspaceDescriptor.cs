using Domain;

namespace Application.Descriptors
{
   public interface IWorkspaceDescriptor
   {
      StringKey WorkspaceKey { get; }
      Type ComponentType { get; }
   }
}
