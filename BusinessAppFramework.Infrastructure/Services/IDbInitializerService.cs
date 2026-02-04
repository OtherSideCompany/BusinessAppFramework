using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.Services
{
   public interface IDbInitializerService
   {
      Task InitializeDatabaseAsync();
   }
}
