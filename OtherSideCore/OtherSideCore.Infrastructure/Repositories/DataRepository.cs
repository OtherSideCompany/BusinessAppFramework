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

      #region Public Methods

      public abstract Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      protected void LogGetAllAsync(List<string> filters, bool extendedSearch)
      {
         if (filters == null || !filters.Any())
         {
            _logger.LogInformation("{Type}, {MethodName}, filters : {Filters}, extendedSearch : {ExtendedSearch}",
            GetType(), nameof(GetAllAsync), "none", extendedSearch.ToString());
         }
         else
         {
            _logger.LogInformation("{Type}, {MethodName}, filters : {Filters}, extendedSearch : {ExtendedSearch}",
            GetType(), nameof(GetAllAsync), string.Join(',', filters), extendedSearch.ToString());
         }         
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
            T entity = await context.Set<T>().FindAsync(entityId);

            if (entity != null)
            {
               entity.SetProperties(databaseFields);

               await context.SaveChangesAsync();
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");            
            }
         }
      }

      public async Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetAsync), entityId.ToString());

         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<T>().FindAsync(entityId, cancellationToken);
         }
      }

      public async Task DeleteAsync(int entityId)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteAsync), entityId);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = context.Set<T>().Find(entityId);

            if (entity != null)
            {
               context.Set<T>().Remove(entity);
               await context.SaveChangesAsync();
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");
            }
         }
      }

      public async Task<DateTime> GetModificatonTimeAsync(int entityId)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<T>().FindAsync(entityId);

            if (entity != null)
            {
               return entity.LastModifiedDateTime;
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");
            }
         }
      }

      public void Dispose()
      {

      }

      #endregion
   }
}
