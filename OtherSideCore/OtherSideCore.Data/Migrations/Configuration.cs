using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Migrations
{
   public class Configuration : DbMigrationsConfiguration<OtherSideCoreContext>
   {
      public Configuration()
      {
         AutomaticMigrationsEnabled = false;
      }

      protected override void Seed(OtherSideCoreContext context)
      {
         //  This method will be called after migrating to the latest version.

         //  You can use the DbSet<T>.AddOrUpdate() helper extension method
         //  to avoid creating duplicate seed data.
      }
   }
}
