using OtherSideCore.Domain;

namespace OtherSideCore.Application.Descriptors
{
    public abstract class WorkspaceDescriptor : IWorkspaceDescriptor
    {
        public StringKey WorkspaceKey { get; init; } = StringKey.From("")!;
        public Type ComponentType { get; init; } = default!;
    }
}
