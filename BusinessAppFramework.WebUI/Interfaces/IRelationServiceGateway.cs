using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IRelationServiceGateway
    {
        Task SetParentAsync(int parentId, int childId, string key);
        Task<DomainObjectReference?> GetHydratedReferenceAsync(int parentId, int childId, string key);
        Task<DomainObjectReference?> GetHydratedDomainObjectReference(int domainObjectId, string relationKeyReference);
        Task<DomainObjectReferenceListItem?> GetHydratedDomainObjectReferenceListItem(int domainObjectReferenceListItemId, string relationKeyReference);
        Task<List<int>> GetChildrenIdsAsync(int parentId, string key);
    }
}
