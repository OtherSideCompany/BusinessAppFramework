namespace BusinessAppFramework.Application.Services
{
   public interface IGlobalDataService
   {
      Task LoadGlobalDataAsync();
      void UnloadData();
   }
}
