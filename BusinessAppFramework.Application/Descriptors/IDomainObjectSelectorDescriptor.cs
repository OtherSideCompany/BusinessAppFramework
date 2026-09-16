namespace BusinessAppFramework.Application.Descriptors
{
    public interface IDomainObjectSelectorDescriptor : IWorkspaceDescriptor
    {
        Type DomainObjectType { get; }
        Type SearchResultType { get; }
        Type SearchListTemplateProviderType { get; }
        List<string> ConstraintKeys { get; }
        string DefaultConstraintKey { get; }
    }
}
