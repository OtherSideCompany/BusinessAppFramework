using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Relations;
using OtherSideCore.Application.Repository;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Factories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
    public class Repository<TDomainObject, TEntity> : IDisposable, IRelationRepository, IRepository<TDomainObject>
        where TDomainObject : DomainObject, new()
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
            _relationResolver = repositoryDependencies.RelationResolver;
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

                    if (_relationResolver.ContainsParentChildRelationByChildType(typeof(TEntity), parentType))
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
                _mapper.Map(entity, domainObject);
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

        public virtual async Task DeleteAsync(int domainObjectId)
        {
            _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteAsync), domainObjectId);

            using (var context = _dbContextFactory.CreateDbContext())
            {
                int affectedRows = 0;

                if (_canUseExecuteDelete)
                {
                    affectedRows = await context.Set<TEntity>().Where(e => e.Id == domainObjectId).ExecuteDeleteAsync();
                }
                else
                {
                    var entity = await context.Set<TEntity>().FindAsync(domainObjectId);

                    if (entity != null)
                    {
                        context.Remove(entity);
                        affectedRows = 1;
                    }

                    await context.SaveChangesAsync();
                }

                if (affectedRows == 0)
                {
                    throw new ArgumentNullException($"Entity with Id {domainObjectId} not found in data repository {nameof(TEntity).ToString()}");
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

        public virtual async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId)
        {
            _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetDomainObjectReferencesAsync), domainObjectId);

            var domainObjectReferences = new List<DomainObjectReference>();

            using var context = _dbContextFactory.CreateDbContext();

            foreach (var relationEntry in _relationResolver.GetReferenceRelationEntriesBySourceType(typeof(TEntity)))
            {
                var entity = await context.Set<TEntity>().FindAsync(domainObjectId);

                var relatedId = (int?)relationEntry.EntityIdProperty.GetValue(entity);

                domainObjectReferences.Add(new DomainObjectReference(relationEntry.RelationKey.Key, relatedId));
            }

            return domainObjectReferences;
        }

        public virtual async Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync(int domainObjectId)
        {
            _logger.LogInformation($"{GetType()}, {nameof(GetDomainObjectReferenceListsAsync)}, domainObjectId : {domainObjectId}");

            var domainObjectReferenceLists = new List<DomainObjectReferenceList>();

            using var context = _dbContextFactory.CreateDbContext();

            foreach (var relationEntry in _relationResolver.GetReferenceListRelationEntriesBySourceType(typeof(TEntity)))
            {
                var entity = await context.Set<TEntity>().FindAsync(domainObjectId);

                var collectionEntry = context.Entry(entity).Collection(relationEntry.EntityProperty.Name);

                if (!collectionEntry.IsLoaded)
                    await collectionEntry.LoadAsync();

                var collectionObject = relationEntry.EntityProperty.GetValue(entity)!;

                var entities = ((System.Collections.IEnumerable)collectionObject).Cast<IEntity>().ToList();
                var relatedIds = entities.Select(e => e.Id).ToList();

                domainObjectReferenceLists.Add(new DomainObjectReferenceList(relationEntry.RelationKey.Key, relatedIds));
            }

            return domainObjectReferenceLists;
        }

        public async Task HydrateDomainObjectReferenceAsync(DomainObjectReference domainObjectReference)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceAsync)}, domainObjectReferenceId : {domainObjectReference.DomainObjectId}, key : {domainObjectReference.RelationKey}");

            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<TEntity>().FindAsync(domainObjectReference.DomainObjectId);

            if (entity != null && _relationResolver.TryGetReferenceRelationEntry(StringKey.From(domainObjectReference.RelationKey), out var relationEntry))
            {
                domainObjectReference.DisplayValue = entity.ToString();
            }
        }

        public async Task HydrateDomainObjectReferenceListAsync(DomainObjectReferenceList domainObjectReferenceList)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListAsync)}, key : {domainObjectReferenceList.RelationKey}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(domainObjectReferenceList.RelationKey), out var relationListEntry))
            {
                MethodInfo _hydrateTypedMethod = typeof(Repository<TDomainObject, TEntity>).GetMethod(nameof(HydrateDomainObjectReferenceListTypedAsync), BindingFlags.Instance | BindingFlags.NonPublic)!;

                var task = (Task)_hydrateTypedMethod
                           .MakeGenericMethod(relationListEntry.TargetEntityType)
                           .Invoke(this, new object[] { domainObjectReferenceList, context })!;

                await task;
            }
        }

        public async Task HydrateDomainObjectReferenceListItemAsync(DomainObjectReferenceListItem domainObjectReferenceListItem, string relationKey)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListItemAsync)}, domainObjectId : {domainObjectReferenceListItem.DomainObjectId}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(relationKey), out var relationListEntry))
            {
                MethodInfo _hydrateTypedMethod = typeof(Repository<TDomainObject, TEntity>).GetMethod(nameof(HydrateDomainObjectReferenceListItemTypedAsync), BindingFlags.Instance | BindingFlags.NonPublic)!;

                var task = (Task)_hydrateTypedMethod
                           .MakeGenericMethod(relationListEntry.TargetEntityType)
                           .Invoke(this, new object[] { domainObjectReferenceListItem, context })!;

                await task;
            }
        }

        public virtual async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetDomainObjectReferencesAsync), domainObjectId);

            var domainObjectReferences = new List<DomainObjectReference>();

            if (_relationResolver.TryGetReferenceRelationEntry(relationKey, out var relationEntry))
            {
                using var context = _dbContextFactory.CreateDbContext();

                var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

                var relatedId = (int?)relationEntry.EntityIdProperty.GetValue(entity);

                if (relatedId != null)
                {
                    var related = await context.FindAsync(relationEntry.TargetEntityType, relatedId.Value);

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

            if (_relationResolver.TryGetReferenceRelationEntry(relationKey, out var relationEntry))
            {
                using var context = _dbContextFactory.CreateDbContext();

                var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

                relationEntry.EntityIdProperty.SetValue(entity, domainObjectReferenceId);

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(DeleteDomainObjectReferenceAsync), domainObjectId);

            if (_relationResolver.TryGetReferenceRelationEntry(relationKey, out var relationEntry))
            {
                using var context = _dbContextFactory.CreateDbContext();

                var entity = await context.Set<TEntity>().FindAsync(domainObjectId, cancellationToken);

                relationEntry.EntityIdProperty.SetValue(entity, null);

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

        public virtual async Task<int?> GetParentIdAsync<U>(int childDomainObjectId, CancellationToken cancellationToken = default) where U : DomainObject
        {
            _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(GetParentIdAsync));

            return await Task.FromResult<int?>(null);
        }

        public async Task<bool> IsSystemObjectAsync(int domainObjectId)
        {
            _logger.LogInformation($"{GetType()}, {nameof(IsSystemObjectAsync)}, domainObjectId : {domainObjectId}");

            if (!typeof(TEntity).IsAssignableFrom(typeof(ISystemObject)))
                return false;

            using var context = _dbContextFactory.CreateDbContext();

            var systemCode = await context.Set<TEntity>().Where(e => e.Id == domainObjectId).Select(e => ((ISystemObject)e).SystemCode).FirstOrDefaultAsync();

            return systemCode != null;
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
            _relationResolver.SetParentChildRelation(entity, parentType, parent.Id);
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

                if (IsValueTypeOrString(domainObjectProperty.PropertyType))
                {
                    MapProperty(domainObjectProperty.Name, domainObjectPropertyValue, entity, entityProps);
                }
                else if (IsDomainObjectReference(domainObjectProperty.PropertyType))
                {
                    MapDomainObjectReferenceProperty(domainObjectProperty, domainObjectPropertyValue, entity, entityProps);
                }
                else if (IsDomainObjectReferenceList(domainObjectProperty.PropertyType))
                {
                    await MapDomainObjectReferenceListPropertyAsync(domainObjectProperty, domainObjectPropertyValue, entity, entityProps, context);
                }
                else if (IsCollectionButNotString(domainObjectProperty.PropertyType))
                {
                    throw new ArgumentException($"Cannot map collection property '{domainObjectProperty.Name}' of type {domainObjectProperty.PropertyType} in {typeof(TDomainObject).Name}. " +
                                                 "Collections are not supported in this repository. Please use a different mapping strategy.");
                }
                else
                {
                    throw new ArgumentException($"Property type {domainObjectProperty.PropertyType} is unsupported for mappinng between DomainObject and Entities");
                }
            }
        }

        private void MapDomainObjectReferenceProperty(
           PropertyInfo domainObjectReferenceProperty,
           object domainObjectReferencePropertyValue,
           TEntity entity,
           Dictionary<string, PropertyInfo> entityProperties)
        {
            var reference = (DomainObjectReference)domainObjectReferencePropertyValue;

            if (_relationResolver.TryGetReferenceRelationEntry(StringKey.From(reference.RelationKey), out var relationEntry))
            {
                relationEntry.EntityIdProperty.SetValue(entity, reference.DomainObjectId);
            }
        }

        private async Task MapDomainObjectReferenceListPropertyAsync(
           PropertyInfo domainObjectReferenceProperty,
           object domainObjectReferencePropertyValue,
           TEntity entity,
           Dictionary<string, PropertyInfo> entityProperties,
           DbContext context)
        {
            var referenceList = (DomainObjectReferenceList)domainObjectReferencePropertyValue;

            if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(referenceList.RelationKey), out var relationEntry))
            {
                var desiredIds = referenceList.Items.Select(r => r.DomainObjectId).ToHashSet();

                var collectionEntry = context.Entry(entity).Collection(relationEntry.EntityProperty.Name);

                if (!collectionEntry.IsLoaded)
                    await collectionEntry.LoadAsync();

                var collectionObject = relationEntry.EntityProperty.GetValue(entity)!;

                var entities = ((System.Collections.IEnumerable)collectionObject).Cast<IEntity>().ToList();
                var currentIds = entities.Select(e => e.Id).ToHashSet();

                var collectionList = (System.Collections.IList)collectionObject;

                var toRemove = currentIds.Except(desiredIds).ToList();
                var toAdd = desiredIds.Except(currentIds).ToList();

                foreach (var id in toRemove)
                {
                    var existing = entities.First(e => e.Id == id);
                    collectionList.Remove(existing);
                }

                foreach (var id in toAdd)
                {
                    var stub = (IEntity)Activator.CreateInstance(relationEntry.TargetEntityType)!;
                    stub.Id = id;

                    context.Attach(stub);

                    collectionList.Add(stub);
                }
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

        private bool IsDomainObjectReference(Type type)
        {
            return typeof(DomainObjectReference).IsAssignableFrom(type);
        }

        private bool IsDomainObjectReferenceList(Type type)
        {
            return typeof(DomainObjectReferenceList).IsAssignableFrom(type);
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

        private async Task HydrateDomainObjectReferenceListTypedAsync<TargetEntity>(DomainObjectReferenceList domainObjectReferenceList, DbContext context) where TargetEntity : class, IEntity
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListTypedAsync)}, entity type : {typeof(TargetEntity)}");

            var ids = domainObjectReferenceList.Items.Select(r => r.DomainObjectId).ToList();
            var entities = await context.Set<TargetEntity>().AsNoTracking().Where(e => ids.Contains(e.Id)).ToListAsync();

            var map = entities.ToDictionary(e => e.Id, e => e.ToString() ?? string.Empty);

            foreach (var reference in domainObjectReferenceList.Items)
            {
                reference.DisplayValue = map.TryGetValue(reference.DomainObjectId, out var display) ? display : string.Empty;
            }
        }

        private async Task HydrateDomainObjectReferenceListItemTypedAsync<TargetEntity>(DomainObjectReferenceListItem domainObjectReferenceListItem, DbContext context) where TargetEntity : class, IEntity
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListTypedAsync)}, entity type : {typeof(TargetEntity)}");

            var entity = await context.Set<TargetEntity>().AsNoTracking().Where(e => e.Id == domainObjectReferenceListItem.DomainObjectId).FirstOrDefaultAsync();

            if (entity != null)
            {
                domainObjectReferenceListItem.DisplayValue = entity.ToString();
            }
        }


        #endregion
    }
}
