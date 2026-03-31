namespace BusinessAppFramework.Application.Interfaces
{
   public interface IGlobalDataService
   {
      Task LoadGlobalDataAsync();
      void UnloadData();
   }
}
