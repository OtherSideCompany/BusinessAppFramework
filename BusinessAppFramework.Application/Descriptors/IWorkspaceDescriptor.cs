using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Descriptors
{
   public interface IWorkspaceDescriptor
   {
      StringKey WorkspaceKey { get; }
      Type ComponentType { get; }
   }
}
