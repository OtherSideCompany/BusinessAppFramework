using System.Threading.Tasks;

namespace Infrastructure.Services
{
   public interface IDbInitializerService
   {
      Task InitializeDatabaseAsync();
   }
}
