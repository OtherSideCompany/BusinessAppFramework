using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Descriptors
{
   public interface IWorkspaceDescriptor
   {
      string WorkspaceKey { get; }
      Type ComponentType { get; }
   }
}
