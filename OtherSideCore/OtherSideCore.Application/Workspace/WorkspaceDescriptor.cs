using OtherSideCore.Domain;

namespace OtherSideCore.Application.Workspace
{
    public abstract class WorkspaceDescriptor
    {
        public StringKey WorkspaceKey { get; init; } = StringKey.From("")!;
        public Type ComponentType { get; init; } = default!;
    }
}
