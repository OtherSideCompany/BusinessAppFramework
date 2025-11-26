using OtherSideCore.Application.ActionResult;
using OtherSideCore.Application.Workspace;

namespace OtherSideCore.Application.Browser
{
    public class DomainObjectBrowserDescriptor : WorkspaceDescriptor
    {
        public Type SearchListTemplateProviderType { get; init; } = default!;
        public Type? EditorComponentType { get; init; } = default;
        public Type? DetailEditorComponentType { get; init; } = default;
        public List<ActionResult.ApplicationAction> Actions { get; init; } = new();
    }
}
