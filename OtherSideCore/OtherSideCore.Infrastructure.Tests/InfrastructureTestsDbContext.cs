using Microsoft.EntityFrameworkCore;
using OtherSideCore.Infrastructure.Context;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Tests
{
   public class InfrastructureTestsDbContext : SqliteDbContext
   {
      public virtual DbSet<TestEntity> TestEntities { get; set; }
      public virtual DbSet<User> Users { get; set; }

      public InfrastructureTestsDbContext() : base()
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
