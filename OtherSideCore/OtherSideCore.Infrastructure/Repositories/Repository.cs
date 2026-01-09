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
    public class Repository<TDomainObject, TEntity> : IDisposable, IRepository<TDomainObject>
        where TDomainObject : DomainObject, new()
        where TEntity : class, IEntity, new()
    {
        #region Fields

        protected RepositoryDependencies _repositoryDependencies;

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

        public async Task CreateAsync(TDomainObject domainObject)
        {
            _logger.LogInformation("{Type}, {MethodName}", GetType(), nameof(CreateAsync));

            using (var context = _dbContextFactory.CreateDbContext())
            {
                var entity = _mapper.Map<TEntity>(domainObject);

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

                var entities = ((IEnumerable)collectionObject).Cast<IEntity>().ToList();
                var currentIds = entities.Select(e => e.Id).ToHashSet();

                var collectionList = (IList)collectionObject;

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

        #endregion
    }
}
