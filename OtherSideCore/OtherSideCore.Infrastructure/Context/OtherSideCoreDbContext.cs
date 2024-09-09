using Microsoft.EntityFrameworkCore;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class OtherSideCoreDbContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string) }));

         modelBuilder.Entity<EntityBase>().UseTpcMappingStrategy();

         modelBuilder.Entity<User>().UseTpcMappingStrategy();
         modelBuilder.Entity<User>().HasOne(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedById).OnDelete(DeleteBehavior.Restrict);
         modelBuilder.Entity<User>().HasOne(u => u.LastModifiedBy).WithMany().HasForeignKey(u => u.LastModifiedById).OnDelete(DeleteBehavior.Restrict);
      }
   }
}
