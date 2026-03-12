using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.Repositories
{
    public static class IndexableRepository
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public static async Task<int> GetNewIndexAsync<T>(
            ILogger logger,
            IDbContextFactory<DbContext> dbContextFactory) 
            where T : class, IIndexable, IEntity, new()
        {
            logger.LogInformation($"{typeof(T)}, {nameof(GetNewIndexAsync)}");

            using var context = dbContextFactory.CreateDbContext();

            return await context.Set<T>().MaxAsync(t => (int?)t.Index).ContinueWith(t => (t.Result ?? 0) + 1);
        }

        public static async Task<int> GetNewYearIndexAsync<T>(
            ILogger logger,
            IDbContextFactory<DbContext> dbContextFactory,
            int year)
            where T : class, IIndexable, IEntity, new()
        {
            logger.LogInformation($"{typeof(T)}, {nameof(GetNewIndexAsync)}");

            using var context = dbContextFactory.CreateDbContext();

            return await context.Set<T>()
                                .Where(i => i.HistoryInfo.CreationDate.Year == year)
                                .MaxAsync(t => (int?)t.Index)
                                .ContinueWith(t => (t.Result ?? 0) + 1);
        }

        public static async Task<int> GetNewIndexInParentAsync<T>(
            ILogger logger,
            IDbContextFactory<DbContext> dbContextFactory,
            IRelationService relationService,
            int parentId,
            string relationKey)
            where T : class, IIndexable, IEntity, new()
        {
            logger.LogInformation($"{typeof(T)}, {nameof(GetNewIndexAsync)}");

            using var context = dbContextFactory.CreateDbContext();

            var maxIndex = await relationService.GetMaxChildIndexAsync(parentId, relationKey);
            return maxIndex == null ? 0 : (maxIndex.Value + 1);
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
