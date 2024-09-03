using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Repositories
{
   public abstract class DataRepository<T> : IDisposable, IDataRepository<T> where T : EntityBase, new()
   {
      #region

      protected IDbContextFactory<DbContext> _dbContextFactory { get; set; }
      protected ILoggerFactory _loggerFactory { get; set; }
      protected ILogger<DataRepository<T>> _logger { get; set; }

      #endregion

      #region Contructor

      public DataRepository(IDbContextFactory<DbContext> dbContextFactory, ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _loggerFactory = loggerFactory;
         _logger = loggerFactory.CreateLogger<DataRepository<T>>();
      }

      #endregion

      #region Methods

      public abstract Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      protected void LogGetAllAsync(List<string> filters, bool extendedSearch)
      {
         _logger.LogInformation("{Type}, {MethodName}, filters : {Filters}, extendedSearch : {ExtendedSearch}",
            GetType(), nameof(GetAllAsync), filters.Any() ? string.Join(',', filters) : "none", extendedSearch.ToString());
      }

      public async Task<int> CreateAsync(List<DatabaseField> databaseFields)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entityBase = new T();

            entityBase.SetProperties(databaseFields);

            await context.Set<T>().AddAsync(entityBase);
            await context.SaveChangesAsync();

            return entityBase.Id;
         }
      }

      public async Task SaveAsync(int entityId, List<DatabaseField> databaseFields)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}, databaseFields : {DatabaseFields}",
                                GetType(),
                                nameof(SaveAsync),
                                entityId,
                                string.Join(", ", databaseFields.Select(dbf => dbf.DatabaseFieldName + " = " + dbf.GetFormattedValue())));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            T entity = context.Set<T>().First(u => u.Id == entityId);

            entity.SetProperties(databaseFields);

            await context.SaveChangesAsync();
         }
      }

      public async Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetAsync), entityId.ToString());

         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<T>()
                                .FirstAsync(e => e.Id == entityId, cancellationToken);
         }
      }

      public async Task DeleteAsync(int entityId)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteAsync), entityId);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            context.Set<T>().Remove(context.Set<T>().Find(entityId));
            await context.SaveChangesAsync();
         }
      }

      public async Task<DateTime> GetModificatonTimeAsync(int entityId)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<T>().Where(e => e.Id == entityId).Select(e => e.LastModifiedDateTime).FirstAsync();
         }
      }

      public void Dispose()
      {

      }

      #endregion
   }
}
