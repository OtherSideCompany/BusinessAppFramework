using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using OtherSideCore.Infrastructure;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class SqliteDbContext : DbContext
   {
      public string DatabasePath { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder options)
      {
         var directoryPath = Path.GetDirectoryName(DatabasePath);

         if (!Path.Exists(directoryPath))
         {
            Directory.CreateDirectory(directoryPath);
         }

         var connection = CreateConfiguredConnection();
         options.UseSqlite(connection);
      }
      private SqliteConnection CreateConfiguredConnection()
      {
         var connection = new SqliteConnection("Data Source=" + DatabasePath);
         connection.Open();

         connection.CreateFunction("Levenshtein", (string s, string t) => Utils.LevenshteinDistance(s, t));
         connection.CreateFunction("EditDistance", (string s, string t) => Utils.EditDistance(s, t));

         return connection;
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string) }));

         modelBuilder.Entity<EntityBase>().UseTpcMappingStrategy();

         modelBuilder.Entity<User>().UseTpcMappingStrategy();
         modelBuilder.Entity<User>().HasOne(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedById).OnDelete(DeleteBehavior.Restrict);
         modelBuilder.Entity<User>().HasOne(u => u.LastModifiedBy).WithMany().HasForeignKey(u => u.LastModifiedById).OnDelete(DeleteBehavior.Restrict);
      }

      public static async Task MigrateAsync(SqliteDbContext context)
      {
         await context.Database.MigrateAsync();
      }
   }
}
