using OtherSideCore.Application.ActionResult;
using OtherSideCore.Application.Workspace;

namespace OtherSideCore.Application.Browser
{
    public class DomainObjectBrowserDescriptor : WorkspaceDescriptor
    {       
        public Type DomainObjectType { get; init; } = default!;
        public Type SearchResultType { get; init; } = default!;
        public Type SearchListComponentType { get; init; } = default!;
        public Type? EditorComponentType { get; init; } = default;
        public Type? DetailEditorComponentType { get; init; } = default;
        public List<ActionResult.ApplicationAction> Actions { get; init; } = new();
    }
}
