using Microsoft.EntityFrameworkCore;
using OtherSideCore.Infrastructure.Context;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests
{
   public class InfrastructureTestsDbContext : SqliteDbContext
   {
      public virtual DbSet<TestEntity> TestEntities { get; set; }
      public virtual DbSet<User> Users { get; set; }

      public InfrastructureTestsDbContext(string databasePath) : base(databasePath)
      {
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<User>().Metadata.SetDiscriminatorValue("UserDiscriminator");      
      }

      public void ReleaseInstance()
      {
         Dispose();
         Database.EnsureDeleted();
      }

      public override void Dispose()
      {
         ChangeTracker.Clear();
      }
      
   }
}
