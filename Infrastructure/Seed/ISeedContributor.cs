using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Seed
{
   public interface ISeedContributor
   {
      Task SeedDatabaseAsync(DbContext dbContext);
   }
}
