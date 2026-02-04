using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Descriptors
{
   public abstract class WorkspaceDescriptor : IWorkspaceDescriptor
   {
      public StringKey WorkspaceKey { get; init; } = StringKey.From("")!;
      public Type ComponentType { get; init; } = default!;
   }
}
