using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Domain.DomainObjects;
using AutoMapper;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using System.Reflection;
using System.Collections;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class Repository<TDomainObject, TEntity> : IDisposable, IRepository<TDomainObject> where TDomainObject : DomainObject, new()
                                                                                             where TEntity : EntityBase, new()
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory { get; set; }
      protected ILoggerFactory _loggerFactory { get; set; }
      protected ILogger<Repository<TDomainObject, TEntity>> _logger { get; set; }
      protected IMapper _mapper { get; set; }

      #endregion

      #region Contructor

      public Repository(IDbContextFactory<DbContext> dbContextFactory, IMapper mapper, ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _loggerFactory = loggerFactory;
         _logger = loggerFactory.CreateLogger<Repository<TDomainObject, TEntity>>();
         _mapper = mapper;
      }

      #endregion

      #region Public Methods

      public async Task<List<TDomainObject>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entities = await context.Set<TEntity>().ToListAsync(cancellationToken);

            return _mapper.Map<List<TDomainObject>>(entities);
         }
      }

      public async Task<List<TDomainObject>> GetAllAsync(Expression<Func<TDomainObject, bool>> where,
                                                         CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entities = await context.Set<TEntity>().ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                       .Where(where)
                                                       .ToListAsync();

            return _mapper.Map<List<TDomainObject>>(entities);
         }
      }

      public async Task<List<TDomainObject>> GetAllPaginatedAsync(Expression<Func<TDomainObject, bool>> where,
                                                                  int pageNumber,
                                                                  int pageSize,
                                                                  CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entities = await context.Set<TEntity>().ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                       .Where(where)
                                                       .Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToListAsync();

            return _mapper.Map<List<TDomainObject>>(entities);
         }
      }

      public async Task<int> CountAsync(Expression<Func<TDomainObject, bool>> predicate, CancellationToken cancellationToken)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<TEntity>().ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                               .CountAsync(predicate, cancellationToken);
         }
      }

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

      public async Task CreateAsync(TDomainObject domainObject, int userId)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = _mapper.Map<TEntity>(domainObject);

            entity.CreationDate = DateTime.Now;
            entity.LastModifiedDateTime = DateTime.Now;
            entity.CreatedById = userId;
            entity.LastModifiedById = userId;

            DetachVirtualProperties(entity, context);

            context.Entry(entity).State = EntityState.Detached;

            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();

            _mapper.Map(entity, domainObject);
         }
      }

      public async Task SaveAsync(TDomainObject domainObject, int userId)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}",
                                GetType(),
                                nameof(SaveAsync),
                                domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            TEntity existingEntity = await context.Set<TEntity>().FindAsync(domainObject.Id);

            if (existingEntity != null)
            {
               var entity = _mapper.Map<TEntity>(domainObject);

               entity.LastModifiedDateTime = DateTime.Now;
               entity.LastModifiedById = userId;

               context.Entry(existingEntity).CurrentValues.SetValues(entity);
               await context.SaveChangesAsync();

               _mapper.Map(entity, domainObject);
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public async Task<TDomainObject> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetAsync), domainObjectId.ToString());

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

            return _mapper.Map<TDomainObject>(entity);
         }
      }

      public async Task LoadAsync(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(LoadAsync), domainObject.Id.ToString());

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>().FindAsync(domainObject.Id);

            _mapper.Map(entity, domainObject);
         }
      }

      public async Task DeleteAsync(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteAsync), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var existingEntity = await context.Set<TEntity>().FindAsync(domainObject.Id);

            if (existingEntity != null)
            {
               context.Set<TEntity>().Remove(existingEntity);
               await context.SaveChangesAsync();

               domainObject.Id = 0;
               domainObject.LastModifiedBy = null;
               domainObject.CreatedBy = null;
               domainObject.CreationDate = default;
               domainObject.LastModifiedDateTime = default;
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public async Task<DateTime> GetLastModificatonTimeAsync(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetLastModificatonTimeAsync), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var existingEntity = await context.Set<TEntity>().FindAsync(domainObject.Id);

            if (existingEntity != null)
            {
               return existingEntity.LastModifiedDateTime;
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public async Task LoadTrackingInfos(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(LoadTrackingInfos), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>()
                                      .Include(e => e.CreatedBy)
                                      .Include(e => e.LastModifiedBy)
                                      .FirstOrDefaultAsync(e => e.Id == domainObject.Id);

            if (entity.CreatedBy != null)
            {
               domainObject.CreatedBy = MapIfNull(entity.CreatedBy, domainObject.CreatedBy);
            }
            else
            {
               domainObject.CreatedBy = null;
            }

            if (entity.LastModifiedBy != null)
            {
               domainObject.LastModifiedBy = MapIfNull(entity.LastModifiedBy, domainObject.LastModifiedBy);
            }
            else
            {
               domainObject.LastModifiedBy = null;
            }
         }
      }

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods

      private TDestination MapIfNull<TSource, TDestination>(TSource source, TDestination destination)
      {
         // Check if destination is null, if so, create a new instance using AutoMapper
         if (destination == null && source != null)
         {
            destination = _mapper.Map<TDestination>(source);
         }
         else if (source != null)
         {
            _mapper.Map(source, destination); // Perform regular mapping if destination is not null
         }

         return destination;
      }

      private void DetachVirtualProperties(TEntity entity, DbContext dbContext)
      {
         var properties = typeof(TEntity).GetProperties();

         foreach (var property in properties)
         {
            if (IsVirtualProperty(property))
            {
               var virtualProperty = property.GetValue(entity);

               if (virtualProperty != null)
               {
                  if (virtualProperty is ICollection collection)
                  {
                     foreach (var item in collection)
                     {
                        dbContext.Entry(item).State = EntityState.Unchanged;
                     }
                  }
                  else
                  {
                     dbContext.Entry(virtualProperty).State = EntityState.Unchanged;
                  }
               }
            }
         }
      }

      private bool IsVirtualProperty(PropertyInfo property)
      {
         return property.GetGetMethod().IsVirtual;
      }

      #endregion
   }
}
