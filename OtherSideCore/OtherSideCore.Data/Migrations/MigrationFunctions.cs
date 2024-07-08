using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Migrations
{
   public static class MigrationFunctions
   {
      private static string ReadSqlScript(string path)
      {
         var assembly = Assembly.GetExecutingAssembly();

         using (var stream = assembly.GetManifestResourceStream(path))
         {
            using (var reader = new StreamReader(stream))
            {
               return reader.ReadToEnd();
            }
         }
      }

      public static string CreateTSqlLevenshteinDistanceFunctionScript()
      {
         return ReadSqlScript("OtherSideCore.Data.Migrations.CreateTSqlLevenshteinDistanceFunction.sql");
      }

      public static string CreateTSqlEditDistanceFunctionScript()
      {
         return ReadSqlScript("OtherSideCore.Data.Migrations.CreateTSqlEditDistanceFunction.sql");
      }
   }
}
