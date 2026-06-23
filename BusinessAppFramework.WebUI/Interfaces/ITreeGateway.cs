using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface ITreeGateway
    {
        Task<Tree?> GetTreeAsync(int domainObjectId, string treeKey);
        Task<Branch?> GetTreeBranchAsync(int domainObjectId, string treeKey, string relationKey);
        Task<Node?> CreateNode(int parentId, string parentChildRelationKey);
        Task<Node?> GetNode(int parentId, int childId, string parentChildRelationKey);
        Task<bool> DeleteNodeAsync(int parentId, int childId, string parentChildRelationKey);
    }
}
