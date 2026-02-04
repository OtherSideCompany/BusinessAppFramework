namespace Application.Services
{
   public interface IGlobalDataService
   {
      Task LoadGlobalDataAsync();
      void UnloadData();
   }
}
