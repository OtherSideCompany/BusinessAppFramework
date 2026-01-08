using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
    public interface IRelationRepository
    {
        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId);
        Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync(int domainObjectId);
        Task HydrateDomainObjectReferenceAsync(DomainObjectReference domainObjectReference);
        Task HydrateDomainObjectReferenceListAsync(DomainObjectReferenceList domainObjectReferenceList);
        Task HydrateDomainObjectReferenceListItemAsync(DomainObjectReferenceListItem domainObjectReferenceListItem, string relationKey);


        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken);
        Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken);
        Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken);
    }
}
