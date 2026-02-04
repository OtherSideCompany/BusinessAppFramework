using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using System.IO;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class SqliteDbContext : OtherSideCoreDbContext
   {
      public string DatabasePath { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder options)
      {
         if (!string.IsNullOrEmpty(DatabasePath) && !DatabasePath.Equals(":memory:"))
         {
            var directoryPath = Path.GetDirectoryName(DatabasePath);

            if (!Path.Exists(directoryPath))
            {
               Directory.CreateDirectory(directoryPath);
            }
         }

         var connection = CreateConfiguredConnection();
         options.UseSqlite(connection);
      }
      private SqliteConnection CreateConfiguredConnection()
      {
         var connection = new SqliteConnection("Data Source=" + DatabasePath);
         connection.Open();

         connection.CreateFunction("Levenshtein", (string s, string t) => Utils.LevenshteinDistance(s, t));
         connection.CreateFunction("EditDistance", (string s, string t, int maxSearchDistance) => Utils.EditDistance(s, t, maxSearchDistance));

         return connection;
      }

      
   }
}
