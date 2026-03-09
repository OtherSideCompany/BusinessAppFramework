using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IRelationServiceGateway
    {
        Task SetParentAsync(int parentId, int childId, string key);
        Task<DomainObjectReference?> GetHydratedReference(int parentId, int childId, string key);
    }
}
