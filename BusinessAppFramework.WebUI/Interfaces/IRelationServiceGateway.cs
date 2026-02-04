namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IRelationServiceGateway
   {
      Task SetParentAsync(int parentId, int childId, string key);
   }
}
