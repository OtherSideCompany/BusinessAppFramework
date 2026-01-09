using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Runtime.CompilerServices;

namespace OtherSideCore.Application.Relations
{
    public interface IRelationService
    {
        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync<TDomainObject>(int domainObjectId) where TDomainObject : DomainObject;
        Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync<TDomainObject>(int domainObjectId) where TDomainObject : DomainObject;
        Task HydrateDomainObjectReferenceAsync(DomainObjectReference domainObjectReference);
        Task HydrateDomainObjectReferenceListAsync(DomainObjectReferenceList domainObjectReferenceList);
        Task HydrateDomainObjectReferenceListItemAsync(DomainObjectReferenceListItem domainObjectReferenceListItem, string relationKey);
        Task<List<int>> GetChildrenIdsAsync(int parentId, string relationKey, CancellationToken cancellationToken = default);
        Task SetParentAsync(int parentId, int childId, string relationKey, CancellationToken cancellationToken = default);
    }
}
