using Microsoft.EntityFrameworkCore;
using OtherSideCore.Adapter;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Services
{
   public class DbInitializerService : IDbInitializerService
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected IModuleProviderService _moduleProviderService;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DbInitializerService(
         IDbContextFactory<DbContext> dbContextFactory,
         IModuleProviderService moduleProviderService)
      {
         _dbContextFactory = dbContextFactory;
         _moduleProviderService = moduleProviderService;
      }

      #endregion

      #region Public Methods

      public virtual async Task InitializeDatabaseAsync()
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            var levenshteinScript = CreateTSqlLevenshteinDistanceFunctionScript();
            var editDistanceScript = CreateTSqlEditDistanceFunctionScript();

            await context.Database.ExecuteSqlRawAsync(levenshteinScript);
            await context.Database.ExecuteSqlRawAsync(editDistanceScript);

            foreach (var module in _moduleProviderService.GetModules())
            {
               await module.SeedDatabaseAsync(context);
            }
         }         
      }

      public string ReadSqlScript(string path, Assembly assembly)
      {
         using (var stream = assembly.GetManifestResourceStream(path))
         {
            using (var reader = new StreamReader(stream))
            {
               return reader.ReadToEnd();
            }
         }
      }

      #endregion

      #region Private Methods

      private string CreateTSqlLevenshteinDistanceFunctionScript()
      {
         return ReadSqlScript(typeof(DbInitializerService).Namespace + ".CreateTSqlLevenshteinDistanceFunction.sql", Assembly.GetExecutingAssembly());
      }

      private string CreateTSqlEditDistanceFunctionScript()
      {
         return ReadSqlScript(typeof(DbInitializerService).Namespace + ".CreateTSqlEditDistanceFunction.sql", Assembly.GetExecutingAssembly());
      }

      #endregion
   }
}
