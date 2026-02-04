
namespace OtherSideCore.Domain.Services
{
   public interface IGlobalDataService
   {
      Task LoadGlobalDataAsync();
      void UnloadData();
   }
}
