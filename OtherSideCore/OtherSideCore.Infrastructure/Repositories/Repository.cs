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

namespace OtherSideCore.Infrastructure.Repositories
{
   public class Repository<TDomainObject, TEntity> : IDisposable, IRepository<TDomainObject> where TDomainObject : DomainObject, new()
                                                                                             where TEntity : EntityBase, new()
   {
      #region Fields

      protected List<Type> _supportedDomainObjectParentTypes;

      protected IDbContextFactory<DbContext> _dbContextFactory { get; set; }
      protected ILoggerFactory _loggerFactory { get; set; }
      protected ILogger<Repository<TDomainObject, TEntity>> _logger { get; set; }
      protected IMapper _mapper { get; set; }

      #endregion

      #region Contructor

      public Repository(IDbContextFactory<DbContext> dbContextFactory, IMapper mapper, ILoggerFactory loggerFactory, List<Type> supportedDomainObjectParentTypes)
      {
         _dbContextFactory = dbContextFactory;
         _loggerFactory = loggerFactory;
         _logger = loggerFactory.CreateLogger<Repository<TDomainObject, TEntity>>();
         _mapper = mapper;

         _supportedDomainObjectParentTypes = new List<Type>();

         if (supportedDomainObjectParentTypes != null)
         { 
            _supportedDomainObjectParentTypes.AddRange(supportedDomainObjectParentTypes); 
         }
         
      }

      #endregion

      #region Public Methods

      public virtual async Task<List<TDomainObject>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         if (parent == null)
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else if (_supportedDomainObjectParentTypes.Contains(parent.GetType()))
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .Where(GetParentRelationPredicate(parent))
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else
         {
            throw new ArgumentException($"Cannot handle parent type {parent.GetType()} for {GetType()}");
         }
      }

      public virtual async Task<List<TDomainObject>> GetAllAsync(Expression<Func<TDomainObject, bool>> where, DomainObject? parent, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         if (parent == null)
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .Where(where)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else if (_supportedDomainObjectParentTypes.Contains(parent.GetType()))
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .Where(GetParentRelationPredicate(parent))
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .Where(where)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else
         {
            throw new ArgumentException($"Cannot handle parent type {parent.GetType()} for {GetType()}");
         }
      }

      public virtual async Task<List<TDomainObject>> GetAllPaginatedAsync(Expression<Func<TDomainObject, bool>> where,
                                                                          DomainObject? parent,
                                                                          int pageNumber,
                                                                          int pageSize,
                                                                          CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         if (parent == null)
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .Where(where)
                                                  .Skip((pageNumber - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else if (_supportedDomainObjectParentTypes.Contains(parent.GetType()))
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .Where(GetParentRelationPredicate(parent))
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .OrderByDescending(e => e.Id)
                                                  .Where(where)
                                                  .Skip((pageNumber - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync(cancellationToken);
            }
         }
         else
         {
            throw new ArgumentException($"Cannot handle parent type {parent.GetType()} for {GetType()}");
         }
      }

      public virtual async Task<int> CountAsync(Expression<Func<TDomainObject, bool>> predicate, DomainObject? parent, CancellationToken cancellationToken)
      {
         if (parent == null)
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .CountAsync(predicate, cancellationToken);
            }
         }
         else if (_supportedDomainObjectParentTypes.Contains(parent.GetType()))
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .Where(GetParentRelationPredicate(parent))
                                                  .ProjectTo<TDomainObject>(_mapper.ConfigurationProvider)
                                                  .CountAsync(predicate, cancellationToken);
            }
         }
         else
         {
            throw new ArgumentException($"Cannot handle parent type {parent.GetType()} for {GetType()}");
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

      public async Task CreateAsync(TDomainObject domainObject, DomainObject? parent, int userId)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = _mapper.Map<TEntity>(domainObject);

            if (parent != null)
            {
               SetParent(entity, parent);
            }

            await CreateEntityAsync(context, entity, userId);

            _mapper.Map(entity, domainObject);
         }
      }

      public virtual async Task SaveAsync(TDomainObject domainObject, int? userId)
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
               _mapper.Map(domainObject, existingEntity);

               existingEntity.LastModifiedDateTime = DateTime.Now;
               existingEntity.LastModifiedById = userId;

               await context.SaveChangesAsync();

               await LoadNavigationPropertiesAsync(context, existingEntity);

               _mapper.Map(existingEntity, domainObject);
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public async Task<TDomainObject> GetAsync(int domainObjectId, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetAsync), domainObjectId.ToString());

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

            await LoadNavigationPropertiesAsync(context, entity);

            return _mapper.Map<TDomainObject>(entity);
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

      public async Task<DateTime> GetLastModificatonTimeAsync(TDomainObject domainObject, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetLastModificatonTimeAsync), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            bool exists = await context.Set<TEntity>().AnyAsync(e => e.Id == domainObject.Id);

            if (exists)
            {
               return await context.Set<TEntity>().AsNoTracking()
                                                  .Where(e => e.Id == domainObject.Id)
                                                  .Select(e => e.LastModifiedDateTime)
                                                  .FirstAsync(cancellationToken);
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods

      protected virtual Expression<Func<TEntity, bool>> GetParentRelationPredicate(DomainObject parent)
      {
         return entity => false;
      }

      protected virtual void SetParent(TEntity entity, DomainObject parent)
      {

      }

      protected async Task CreateEntityAsync(DbContext context, TEntity entity, int userId)
      {
         entity.CreationDate = DateTime.Now;
         entity.LastModifiedDateTime = DateTime.Now;
         entity.CreatedById = userId;
         entity.LastModifiedById = userId;

         await context.Set<TEntity>().AddAsync(entity);

         await LoadNavigationPropertiesAsync(context, entity);

         await context.SaveChangesAsync();
      }

      private async Task LoadNavigationPropertiesAsync(DbContext context, TEntity entity)
      {
         foreach (var navigation in context.Entry(entity).Navigations)
         {
            if (!navigation.IsLoaded)
            {
               await navigation.LoadAsync();
            }
         }
      }

      private TDestination MapIfNull<TSource, TDestination>(TSource source, TDestination destination)
      {
         if (destination == null && source != null)
         {
            destination = _mapper.Map<TDestination>(source);
         }
         else if (source != null)
         {
            _mapper.Map(source, destination);
         }

         return destination;
      }

      #endregion
   }
}
