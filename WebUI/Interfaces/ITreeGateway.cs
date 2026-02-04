using Application.Trees;

namespace WebUI.Interfaces
{
   public interface ITreeGateway
   {
      Task<Tree?> GetTreeAsync(int domainObjectId, string key);
      Task<Node?> CreateNode(int parentId, string parentChildRelationKey);
      Task<bool> DeleteNodeAsync(int parentId, int childId, string parentChildRelationKey);
   }
}
