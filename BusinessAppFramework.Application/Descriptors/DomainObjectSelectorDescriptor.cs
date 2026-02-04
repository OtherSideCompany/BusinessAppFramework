using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Descriptors
{
   public class DomainObjectSelectorDescriptor<TDomainObject, TSearchResult> : WorkspaceDescriptor, IDomainObjectSelectorDescriptor
       where TDomainObject : DomainObject, new()
       where TSearchResult : DomainObjectSearchResult, new()
   {
      public Type DomainObjectType => typeof(TDomainObject);
      public Type SearchResultType => typeof(TSearchResult);
      public Type SearchListTemplateProviderType { get; init; } = default!;

      public DomainObjectSelectorDescriptor()
      {

      }
   }
}
