using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.DomainObjects;
using AutoMapper;
using OtherSideCore.Application.Repository;
using OtherSideCore.Application;
using OtherSideCore.Domain;
using OtherSideCore.Infrastructure.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Relations;
using System.Reflection;

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

            return _mapper.Map<List<TDomainObject>>(entities);
         }
      }

      public virtual async Task<int> CountAsync(DomainObject? parent, CancellationToken cancellationToken)
      {
         //return await CountAsync([], false, _ => true, parent, cancellationToken);
         return await Task.FromResult(0);
      }

      public async Task CreateAsync(TDomainObject domainObject, DomainObject? parent)
      {
         _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = _mapper.Map<TEntity>(domainObject);

            if (parent != null)
            {
               SetParent(entity, parent);
            }

            await CreateEntityAsync(context, entity);
         }
      }

      public virtual async Task SaveAsync(TDomainObject domainObject)
      {
         _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}",
                                GetType(),
                                nameof(SaveAsync),
                                domainObject.Id);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            TEntity existingEntity = await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == domainObject.Id);

            if (existingEntity == null)
            {
               throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(TEntity).ToString()}");
            }

            await CopyEditablePropertiesAsync(domainObject, existingEntity, context);

            await context.SaveChangesAsync();
         }
      }

      public virtual async Task SaveIndexAsync(IIndexable domainObject)
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
                     await context.SaveChangesAsync();
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

            if (affectedRows == 0)
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

      public virtual async Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken)
      {
         _logger.LogInformation($"{GetType()}, {nameof(CreateDomainObjectReferenceAsync)}, relationKey = {relationKey}, domainObjectId = {domainObjectId}, domainObjectReferenceId = {domainObjectReferenceId}");

         if (_relationResolver.TryGetEntry(relationKey, out var relationEntry))
         {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

            relationEntry.SetRelation(entity, domainObjectReferenceId);

            await context.SaveChangesAsync(cancellationToken);
         }            
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

      protected void SetParent(TEntity entity, DomainObject parent)
      {
         var parentType = _repositoryDependencies.DomainObjectEntityTypeMap.GetEntityType(parent.GetType());
         _relationResolver.SetParentChildRelation(entity, parentType, parent.Id, RelationType.ParentChild);
      }

      protected async Task CreateEntityAsync(DbContext context, TEntity entity)
      {
         await context.Set<TEntity>().AddAsync(entity);

         await context.SaveChangesAsync();
      }

      private async Task CopyEditablePropertiesAsync(TDomainObject domainObject, TEntity entity, DbContext context)
      {
         MapHistoryInfo(domainObject, entity);

         var ignored = GetIgnoredDomainObjectMappingProperties();

         var domainObjectProps = typeof(TDomainObject).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && !ignored.Contains(p.Name)).ToDictionary(p => p.Name);
         var entityProps = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite).ToDictionary(p => p.Name);

         foreach (var domainObjectProperty in domainObjectProps.Values)
         {
            var domainObjectPropertyValue = domainObjectProperty.GetValue(domainObject);

            if (IsCollectionButNotString(domainObjectProperty.PropertyType))
            {
               throw new ArgumentException($"Cannot map collection property '{domainObjectProperty.Name}' of type {domainObjectProperty.PropertyType} in {typeof(TDomainObject).Name}. " +
                                            "Collections are not supported in this repository. Please use a different mapping strategy.");
            }
            else if (!IsValueTypeOrString(domainObjectProperty.PropertyType))
            {
               MapDomainObjectProperty(domainObjectProperty, domainObjectPropertyValue, entity, entityProps);
            }
            else
            {
               MapProperty(domainObjectProperty.Name, domainObjectPropertyValue, entity, entityProps);
            }
         }
      }

      private void MapDomainObjectProperty(
         PropertyInfo domainObjectProperty,
         object domainObjectPropertyValue,
         TEntity entity,
         Dictionary<string, PropertyInfo> entityProperties)
      {
         var domainObjectIdProperty = domainObjectProperty.PropertyType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

         var idValue = domainObjectIdProperty.GetValue(domainObjectPropertyValue);
         var foreignKeyName = domainObjectProperty.Name + "Id";

         if (entityProperties.TryGetValue(foreignKeyName, out var foreignKeyProperty))
         {
            foreignKeyProperty.SetValue(entity, idValue);
         }
         else
         {
            throw new ArgumentException($"Property '{foreignKeyName}' not found in target type {typeof(TEntity).Name}");
         }
      }

      private void MapProperty(
         string domainObjectPropertyName, 
         object domainObjectPropertyValue,
         TEntity entity,
         Dictionary<string, PropertyInfo> entityProperties)
      {
         if (entityProperties.TryGetValue(domainObjectPropertyName, out var targetProp))
         {
            targetProp.SetValue(entity, domainObjectPropertyValue);
         }
         else
         {
            throw new ArgumentException($"Property '{domainObjectPropertyName}' not found in target type {typeof(TEntity).Name}");
         }
      }

      private bool IsValueTypeOrString(Type type)
      {
         return type.IsValueType || type == typeof(string);
      }

      private bool IsCollectionButNotString(Type type)
      {
         return typeof(System.Collections.IEnumerable).IsAssignableFrom(type)
                && type != typeof(string);
      }

      private HashSet<string> GetIgnoredDomainObjectMappingProperties()
      {
         return new HashSet<string>
         {
            nameof(DomainObject.CreationDate),
            nameof(DomainObject.CreatedById),
            nameof(DomainObject.CreatedByName),
            nameof(DomainObject.LastModifiedDateTime),
            nameof(DomainObject.LastModifiedById),
            nameof(DomainObject.LastModifiedByName)
         };
      }

      private void MapHistoryInfo(TDomainObject source, TEntity target)
      {
         target.HistoryInfo.CreationDate = source.CreationDate;
         target.HistoryInfo.CreatedById = source.CreatedById;
         target.HistoryInfo.CreatedByName = source.CreatedByName;
         target.HistoryInfo.LastModifiedDateTime = source.LastModifiedDateTime;
         target.HistoryInfo.LastModifiedById = source.LastModifiedById;
         target.HistoryInfo.LastModifiedByName = source.LastModifiedByName;
      }


      #endregion
   }
}
