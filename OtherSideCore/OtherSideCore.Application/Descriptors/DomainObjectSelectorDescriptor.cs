using OtherSideCore.Application.Descriptors;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.WebUI.Descriptors
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
