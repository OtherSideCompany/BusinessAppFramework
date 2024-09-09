using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests
{
   public class InfrastructureTestsDbContextFactory : IDbContextFactory<DbContext>
   {
      private static InfrastructureTestsDbContext _instance;

      public InfrastructureTestsDbContextFactory()
      {
         
      }

      public DbContext CreateDbContext()
      {
         if (_instance == null)
         {
            _instance = new InfrastructureTestsDbContext();
            _instance.DatabasePath = ":memory:";
         }
 
         return _instance;         
      }

      public void ReleaseInstance()
      {
         _instance.ReleaseInstance();
      }
   }
}
