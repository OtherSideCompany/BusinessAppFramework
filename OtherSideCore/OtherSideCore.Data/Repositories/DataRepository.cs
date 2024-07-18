using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace OtherSideCore.Data.Repositories
{
   public abstract class DataRepository<T> : IDisposable, IDataRepository<T> where T : EntityBase, new()
   {
      #region

      protected IDbContextFactory<DbContext> _dbContextFactory { get; set; }

      #endregion

      #region Contructor

      public DataRepository(IDbContextFactory<DbContext> dbContextFactory)
      {
         _dbContextFactory = dbContextFactory;
      }

      #endregion

      #region Methods

      public abstract Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      public async Task<int> CreateAsync(List<DatabaseField> databaseFields)
      {
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
         using (var context = _dbContextFactory.CreateDbContext())
         {
            T entity = context.Set<T>().First(u => u.Id == entityId);

            entity.SetProperties(databaseFields);

            await context.SaveChangesAsync();
         }
      }

      public async Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<T>()
                                .FirstAsync(e => e.Id == entityId, cancellationToken);
         }
      }

      public async Task DeleteAsync(int entityId)
      {
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
