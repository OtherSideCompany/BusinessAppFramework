using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class OtherSideCoreDbContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string), typeof(int) })).HasName("EditDistance");

         modelBuilder.Entity<User>().HasOne(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedById).OnDelete(DeleteBehavior.Restrict);
         modelBuilder.Entity<User>().HasOne(u => u.LastModifiedBy).WithMany().HasForeignKey(u => u.LastModifiedById).OnDelete(DeleteBehavior.Restrict);
      }
   }
}
