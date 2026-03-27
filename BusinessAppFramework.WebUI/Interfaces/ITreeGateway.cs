using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface ITreeGateway
    {
        Task<Tree?> GetTreeAsync(int domainObjectId, string pageTreeKey);
        Task<Branch?> GetTreeBranchAsync(int domainObjectId, string pageTreeKey, string relationKey);
        Task<Node?> CreateNode(int parentId, string parentChildRelationKey);
        Task<bool> DeleteNodeAsync(int parentId, int childId, string parentChildRelationKey);
    }
}
