using OtherSideCore.Application.Descriptors;

namespace OtherSideCore.WebUI.Descriptors
{
    public interface IDomainObjectSelectorDescriptor : IWorkspaceDescriptor
    {
        Type DomainObjectType { get; }
        Type SearchResultType { get; }
        Type SearchListTemplateProviderType { get; }
    }
}
