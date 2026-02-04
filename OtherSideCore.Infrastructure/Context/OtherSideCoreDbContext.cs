using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using System.Linq;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class OtherSideCoreDbContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string), typeof(int) })).HasName("EditDistance");

         RestrictDeleteOnAllForeignKeys(modelBuilder);
      }

      protected void RestrictDeleteOnAllForeignKeys(ModelBuilder modelBuilder)
      {
         foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
         {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
         }
      }
   }
}

