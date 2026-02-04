namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IWorkspaceNavigationService
   {
      Task<List<string>> GetModuleKeysAsync();
      Task<List<string>> GetWorkspaceKeysAsync(string moduleKey);
   }
}
