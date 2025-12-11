using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Repository
{
    public interface IRelationRepository
    {
        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId);
        Task HydrateDomainObjectReferenceAsync(DomainObjectReference domainObjectReference);


        Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken);
        Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken);
        Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken);
    }
}
