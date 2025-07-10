using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Domain.DomainObjects;
using AutoMapper;
using System.Linq.Expressions;
using OtherSideCore.Application.Repository;
using OtherSideCore.Application;
using OtherSideCore.Domain;
using OtherSideCore.Infrastructure.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Relations;
using System.Net.Mime;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class Repository<TDomainObject, TEntity> : IDisposable, IRepository<TDomainObject> where TDomainObject : DomainObject, new()
                                                                                             where TEntity : class, IEntity, new()
   {
      #region Fields

      protected RepositoryDependencies _repositoryDependencies;

      protected IDomainObjectReferenceFactory _domainObjectReferenceFactory;
      protected IDomainObjectReferenceMapFactory _referenceMapFactory;
      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected ILoggerFactory _loggerFactory;
      protected ILogger<Repository<TDomainObject, TEntity>> _logger;
      protected IMapper _mapper;
      protected IRelationResolver _relationResolver;

      protected bool _canUseExecuteDelete = true;

      #endregion

      #region Contructor

      public Repository(
         RepositoryDependencies repositoryDependencies)
      {
         _repositoryDependencies = repositoryDependencies;
         _dbContextFactory = repositoryDependencies.DbContextFactory;
         _loggerFactory = repositoryDependencies.LoggerFactory;
         _logger = repositoryDependencies.LoggerFactory.CreateLogger<Repository<TDomainObject, TEntity>>();
         _mapper = repositoryDependencies.Mapper;
         _domainObjectReferenceFactory = repositoryDependencies.DomainObjectReferenceFactory;
         _referenceMapFactory = repositoryDependencies.ReferenceMapFactory;
         _relationResolver = repositoryDependencies.ParentChildRelationResolver;
      }

      #endregion

      #region Public Methods

      public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId={EntityId}", GetType(), nameof(ExistsAsync), domainObjectId);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<TEntity>().AnyAsync(e => e.Id == domainObjectId, cancellationToken);
         }
      }

      public virtual async Task<List<TDomainObject>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetAllAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var query = context.Set<TEntity>().AsNoTracking();

            if (parent != null)
            {
               var parentType = _repositoryDependencies.DomainObjectEntityTypeMap.GetEntityType(parent.GetType());

               if (_relationResolver.ContainsParentChildRelation(typeof(TEntity), parentType))
               {                  
                  query = query.Where(_relationResolver.GetParentChildRelationPredicate<TEntity>(parent.Id, parentType));
               }
               else
               {
                  throw new ArgumentException($"Cannot handle parent type {parentType} for {GetType()}");
               }
            }

            query = query.OrderByDescending(e => e.Id);

            var entities = await query.ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
               await LoadNavigationPropertiesAsync(context, entity);
            }

            return _mapper.Map<List<TDomainObject>>(entities);
         }
      }

      public virtual async Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken)
      {
         //return await CountAsync([], false, _ => true, parent, cancellationToken);
         return await Task.FromResult(0);
      }

      public async Task CreateAsync(TDomainObject domainObject, DomainObject? parent, int userId, string userName)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = _mapper.Map<TEntity>(domainObject);

            if (parent != null)
            {
               SetParent(entity, parent);
            }

            await CreateEntityAsync(context, entity, userId, userName);

            _mapper.Map(entity, domainObject);
         }
      }

      public virtual async Task SaveAsync(TDomainObject domainObject, int userId, string userName)
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
               await SpecificMapDomainObjectToEntityAsync(domainObject, existingEntity, context);

               existingEntity.HistoryInfo.LastModifiedDateTime = DateTime.Now;
               existingEntity.HistoryInfo.LastModifiedById = userId;
               existingEntity.HistoryInfo.LastModifiedByName = userName;

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

      public virtual async Task SaveIndexAsync(IIndexable domainObject, int userId, string userName)
      {
         if (!(domainObject is TDomainObject))
         {
            throw new ArgumentException("Domain Object type " + domainObject.GetType() + " does not match repository type " + typeof(TDomainObject));
         }

         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}",
                                GetType(),
                                nameof(SaveIndexAsync),
                                ((TDomainObject)domainObject).Id);

         if (domainObject is IIndexable indexableDomainObject)
         {
            using (var context = _dbContextFactory.CreateDbContext())
            {
               TEntity existingEntity = await context.Set<TEntity>().FindAsync(((TDomainObject)domainObject).Id);

               if (existingEntity != null)
               {
                  if (existingEntity is IIndexable existingIndexableEntity)
                  {
                     existingIndexableEntity.Index = indexableDomainObject.Index;

                     existingEntity.HistoryInfo.LastModifiedDateTime = DateTime.Now;
                     existingEntity.HistoryInfo.LastModifiedById = userId;
                     existingEntity.HistoryInfo.LastModifiedByName = userName;

                     await context.SaveChangesAsync();

                     await LoadNavigationPropertiesAsync(context, existingEntity);

                     _mapper.Map(existingEntity, domainObject);
                  }
                  else
                  {
                     throw new ArgumentException("Entity type " + existingEntity.GetType() + " does not implement IIndexable interface");
                  }
               }
               else
               {
                  throw new ArgumentNullException($"Entity with Id {((TDomainObject)domainObject).Id} not found in data repository {nameof(TEntity).ToString()}");
               }
            }
         }
         else
         {
            throw new ArgumentException("Domain Object type " + domainObject.GetType() + " does not implement IIndexable interface");
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

      public virtual async Task DeleteAsync(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteAsync), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            int affectedRows = 0;

            if (_canUseExecuteDelete)
            {
               affectedRows = await context.Set<TEntity>().Where(e => e.Id == domainObject.Id).ExecuteDeleteAsync();
            }
            else
            {
               var entity = await context.Set<TEntity>().FindAsync(domainObject.Id);

               if (entity != null)
               {
                  context.Remove(entity);
                  affectedRows = 1;
               }

               await context.SaveChangesAsync();
            }

            if (affectedRows != 0)
            {
               domainObject.Id = 0;
               domainObject.LastModifiedById = null;
               domainObject.LastModifiedByName = null;
               domainObject.CreatedById = null;
               domainObject.CreatedByName = null;
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
                                                  .Select(e => e.HistoryInfo.LastModifiedDateTime)
                                                  .FirstAsync(cancellationToken);
            }
            else
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }
         }
      }

      public virtual async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetDomainObjectReferencesAsync), domainObjectId);

         var domainObjectReferences = new List<DomainObjectReference>();

         if (_relationResolver.TryGetEntry(relationKey, out var relationEntry))
         {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

            var relatedId = relationEntry.GetRelatedId(entity);

            if (relatedId != null)
            {
               var related = await context.FindAsync(relationEntry.RelatedType, relatedId.Value);

               if (related is IEntity relatedEntity)
               {
                  domainObjectReferences.Add(_domainObjectReferenceFactory.CreateDomainObjectReference(relatedEntity));
               }
            }
         }

         return domainObjectReferences;
      }

      public virtual async Task<DomainObjectReference> CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken)
      {
         throw new NotImplementedException($"Cannot assing reference to type {GetType()}");
      }

      public virtual async Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteDomainObjectReferenceAsync), domainObjectId);

         if (_relationResolver.TryGetEntry(relationKey, out var relationEntry))
         {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

            relationEntry.DeleteRelation(entity, domainObjectReference.DomainObjectId);

            await context.SaveChangesAsync();
         }
      }

      public async Task SetParentAsync(TDomainObject domainObject, DomainObject parent, CancellationToken cancellationToken = default)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(SetParentAsync), domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>().FindAsync(domainObject.Id);

            if (entity != null)
            {
               SetParent(entity, parent);
               await context.SaveChangesAsync(cancellationToken);
            }
         }
      }

      public virtual async Task<int?> GetParentIdAsync<U>(TDomainObject domainObject, CancellationToken cancellationToken = default) where U : DomainObject
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetParentIdAsync));

         return await Task.FromResult<int?>(null);
      }

      public async Task<TDomainObject> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default)
      {
         _logger.LogInformation($"{GetType()}, {nameof(GetFromSystemCodeAsync)}, system code : {systemCode}");

         using (var context = _dbContextFactory.CreateDbContext())
         {
            if (typeof(ISystemObject).IsAssignableFrom(typeof(TEntity)))
            {
               var entity = await context.Set<TEntity>().FirstAsync(so => ((ISystemObject)so).SystemCode != null && ((ISystemObject)so).SystemCode.Equals(systemCode));
               return _mapper.Map<TDomainObject>(entity);
            }
            else
            {
               throw new Exception($"Cannot get system code from type {typeof(TEntity)} as it does not implement ISystemObject interface.");
            }
         }
      }

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods 

      protected virtual async Task SpecificMapDomainObjectToEntityAsync(TDomainObject source, TEntity destination, DbContext dbContext)
      {
         await Task.FromResult(_mapper.Map(source, destination));
      }

      protected void SetParent(TEntity entity, DomainObject parent)
      {
         var parentType = _repositoryDependencies.DomainObjectEntityTypeMap.GetEntityType(parent.GetType());
         _relationResolver.SetRelation(entity, parentType, parent.Id, RelationType.ParentChild);
      }

      protected async Task CreateEntityAsync(DbContext context, TEntity entity, int userId, string userName)
      {
         entity.HistoryInfo.CreationDate = DateTime.Now;
         entity.HistoryInfo.LastModifiedDateTime = DateTime.Now;
         entity.HistoryInfo.CreatedById = userId;
         entity.HistoryInfo.CreatedByName = userName;
         entity.HistoryInfo.LastModifiedById = userId;
         entity.HistoryInfo.LastModifiedByName = userName;

         await context.Set<TEntity>().AddAsync(entity);

         await LoadNavigationPropertiesAsync(context, entity);

         await context.SaveChangesAsync();
      }

      protected async Task LoadNavigationPropertiesAsync(DbContext context, TEntity entity)
      {
         foreach (var navigation in context.Entry(entity).Navigations)
         {
            if (navigation.Metadata.PropertyInfo != null && !navigation.IsLoaded)
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
