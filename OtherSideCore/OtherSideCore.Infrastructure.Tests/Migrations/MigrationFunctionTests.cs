using OtherSideCore.Infrastructure.Migrations;

namespace OtherSideCore.Infrastructure.Tests.Migrations
{
   public class MigrationFunctionTests
   {
      [Fact]
      public void CreateTSqlLevenshteinDistanceFunctionScript_ScriptIsRead()
      {
         var script = MigrationFunctions.CreateTSqlLevenshteinDistanceFunctionScript();

         Assert.False(string.IsNullOrEmpty(script));
      }

      [Fact]
      public void CreateTSqlEditDistanceFunctionScript_ScriptIsRead()
      {
         var script = MigrationFunctions.CreateTSqlEditDistanceFunctionScript();

         Assert.False(string.IsNullOrEmpty(script));
      }
   }
}
