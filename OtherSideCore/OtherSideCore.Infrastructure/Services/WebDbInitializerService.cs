using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Services
{
    public class WebDbInitializerService : IDbInitializerService
    {
        #region Fields

        protected IDbContextFactory<DbContext> _dbContextFactory;

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public WebDbInitializerService(
           IDbContextFactory<DbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
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
