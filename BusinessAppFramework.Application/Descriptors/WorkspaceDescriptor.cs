using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Descriptors
{
   public class WorkspaceDescriptor : IWorkspaceDescriptor
   {
      public string WorkspaceKey { get; init; } = ""!;
      public Type ComponentType { get; init; } = default!;
   }
}
