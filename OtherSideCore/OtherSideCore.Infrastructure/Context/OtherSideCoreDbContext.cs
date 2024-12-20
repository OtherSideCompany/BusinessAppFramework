using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using OtherSideCore.Infrastructure.Entities;
using System.Linq;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class OtherSideCoreDbContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string), typeof(int) })).HasName("EditDistance");

         AutoIncludeNavigationProperties(modelBuilder);
      }

      private void AutoIncludeNavigationProperties(ModelBuilder modelBuilder)
      {
         foreach (var entityType in modelBuilder.Model.GetEntityTypes())
         {
            if (!typeof(User).IsAssignableFrom(entityType.ClrType))
            {
               foreach (var navigation in entityType.GetNavigations())
               {
                  if (!navigation.IsCollection)
                  {
                     modelBuilder.Entity(entityType.ClrType)
                                 .Navigation(navigation.Name)
                                 .AutoInclude();

                     System.Diagnostics.Debug.WriteLine($"AutoIncluding {entityType.ClrType.Name}.{navigation.Name}");
                  }
               }
            }
         }
      }
   }
}

