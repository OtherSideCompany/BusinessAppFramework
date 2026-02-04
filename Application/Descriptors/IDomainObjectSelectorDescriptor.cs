namespace Application.Descriptors
{
   public interface IDomainObjectSelectorDescriptor : IWorkspaceDescriptor
   {
      Type DomainObjectType { get; }
      Type SearchResultType { get; }
      Type SearchListTemplateProviderType { get; }
   }
}
