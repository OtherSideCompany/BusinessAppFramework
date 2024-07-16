using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Internal;

namespace OtherSideCore.Data.Repositories
{
    public abstract class DataRepository<T, U> : IDisposable, IDataRepository<T> where T : EntityBase, new() where U : DbContext
    {
        #region

        protected IDbContextFactory<U> _dbContextFactory { get; set; }

        #endregion

        #region Contructor

        public DataRepository(IDbContextFactory<U> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion

        #region Methods

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
                return await context.Set<T>().FindAsync(entityId, cancellationToken);
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

        public void Dispose()
        {

        }

        #endregion
    }
}
