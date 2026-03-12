using BusinessAppFramework.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.Repositories
{
    public static class StatusRepository
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public static async Task SetStatusAsync<T>(
            ILogger logger,
            IDbContextFactory<DbContext> dbContextFactory,
            int domainObjectId, 
            string statusKey) 
            where T : class, IStatusEntity, new()
        {
            logger.LogInformation($"{typeof(T)}, {nameof(SetStatusAsync)}, domainObjectId: {domainObjectId}");

            using var context = dbContextFactory.CreateDbContext();

            await context.Set<T>().Where(i => i.Id == domainObjectId).ExecuteUpdateAsync(setter => setter.SetProperty(i => i.StatusKey, statusKey));
        }

        public static async Task<string?> GetStatusAsync<T>(
            ILogger logger,
            IDbContextFactory<DbContext> dbContextFactory,
            int domainObjectId) 
            where T : class, IStatusEntity, new()
        {
            logger.LogInformation($"{typeof(T)}, {nameof(GetStatusAsync)}, domainObjectId: {domainObjectId}");

            using var context = dbContextFactory.CreateDbContext();

            return await context.Set<T>().Where(i => i.Id == domainObjectId).Select(i => i.StatusKey).FirstOrDefaultAsync();
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
