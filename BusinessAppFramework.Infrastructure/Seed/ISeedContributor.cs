using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.Seed
{
   public interface ISeedContributor
   {
      Task SeedDatabaseAsync(DbContext dbContext);
   }
}
