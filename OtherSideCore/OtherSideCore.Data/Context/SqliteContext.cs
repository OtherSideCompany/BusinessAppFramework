using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Context
{
   public abstract class SqliteContext : DbContext
   {
      public string DatabasePath { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder options)
      {
         var directoryPath = Path.GetDirectoryName(DatabasePath);

         if (!Path.Exists(directoryPath))
         {
            Directory.CreateDirectory(directoryPath);
         }

         options.UseSqlite($"Data Source={DatabasePath}");
      }

      public static async Task MigrateAsync(SqliteContext context)
      {
         await context.Database.MigrateAsync();
      }
   }
}
