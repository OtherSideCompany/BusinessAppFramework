using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Services
{
    public class RelationService : IRelationService
    {
        #region Fields

        private IDbContextFactory<DbContext> _dbContextFactory;
        private IMapper _mapper;
        protected ILogger<RelationService> _logger;
        private IRelationResolver _relationResolver;
        private IDomainObjectTypeMap _domainObjectTypeMap;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public RelationService(
            IDbContextFactory<DbContext> dbContextFactory,
            IMapper mapper,
            ILoggerFactory loggerFactory,
            IRelationResolver relationResolver,
            IDomainObjectTypeMap domainObjectTypeMap)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<RelationService>();
            _relationResolver = relationResolver;
            _domainObjectTypeMap = domainObjectTypeMap;
        }

        #endregion

        #region Public Methods        

        public async Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync<TDomainObject>(int domainObjectId) where TDomainObject : DomainObject
        {
            _logger.LogInformation($"{GetType()}, {nameof(GetDomainObjectReferenceListsAsync)}, domainObjectId : {domainObjectId}");

            var domainObjectReferenceLists = new List<DomainObjectReferenceList>();

            using var context = _dbContextFactory.CreateDbContext();

            var entityType = _domainObjectTypeMap.GetEntityTypeFromDomainObjectType(typeof(TDomainObject));
            var entity = await context.FindAsync(entityType, domainObjectId);

            foreach (var relationEntry in _relationResolver.GetReferenceListRelationEntriesBySourceType(entityType))
            {
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

        public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync<TDomainObject>(int domainObjectId) where TDomainObject : DomainObject
        {
            _logger.LogInformation("{Type}, {MethodName}, entityId : {EntityId}", GetType(), nameof(GetDomainObjectReferencesAsync), domainObjectId);

            var domainObjectReferences = new List<DomainObjectReference>();

            using var context = _dbContextFactory.CreateDbContext();

            var entityType = _domainObjectTypeMap.GetEntityTypeFromDomainObjectType(typeof(TDomainObject));

            foreach (var relationEntry in _relationResolver.GetReferenceRelationEntriesBySourceType(entityType))
            {
                var entity = await context.FindAsync(entityType, domainObjectId);

                var relatedId = (int?)relationEntry.EntityIdProperty.GetValue(entity);

                domainObjectReferences.Add(new DomainObjectReference(relationEntry.RelationKey.Key, relatedId));
            }

            return domainObjectReferences;
        }

        public async Task HydrateDomainObjectReferenceAsync(DomainObjectReference domainObjectReference)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceAsync)}, domainObjectReferenceId : {domainObjectReference.DomainObjectId}, key : {domainObjectReference.RelationKey}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetReferenceRelationEntry(StringKey.From(domainObjectReference.RelationKey), out var relationEntry))
            {
                var entity = await context.FindAsync(relationEntry.TargetEntityType, domainObjectReference.DomainObjectId);
                domainObjectReference.DisplayValue = entity?.ToString();
            }
        }

        public async Task HydrateDomainObjectReferenceListAsync(DomainObjectReferenceList domainObjectReferenceList)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListAsync)}, key : {domainObjectReferenceList.RelationKey}");

            var domainObjectReferenceListById = domainObjectReferenceList.Items.ToDictionary(i => i.DomainObjectId);
            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(domainObjectReferenceList.RelationKey), out var relationListEntry))
            {
                var ids = domainObjectReferenceList.Items.Select(r => r.DomainObjectId).ToList();

                foreach (var id in ids)
                {
                    var entity = await context.FindAsync(relationListEntry.TargetEntityType, id);
                    domainObjectReferenceListById[id].DisplayValue = entity?.ToString();
                }
            }
        }

        public async Task HydrateDomainObjectReferenceListItemAsync(DomainObjectReferenceListItem domainObjectReferenceListItem, string relationKey)
        {
            _logger.LogInformation($"{GetType()}, {nameof(HydrateDomainObjectReferenceListItemAsync)}, domainObjectId : {domainObjectReferenceListItem.DomainObjectId}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(relationKey), out var relationListEntry))
            {
                var entity = await context.FindAsync(relationListEntry.TargetEntityType, domainObjectReferenceListItem.DomainObjectId);
                domainObjectReferenceListItem.DisplayValue = entity?.ToString();
            }
        }

        public async Task<List<int>> GetChildrenIdsAsync(int parentId, string relationKey, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{GetType()}, {nameof(GetChildrenIdsAsync)}, parentId : {parentId}, relationKey : {relationKey}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(relationKey), out var relationEntry))
            {
                var query = ((IInfrastructureParentChildRelation)relationEntry).GetChildrenIds(context, parentId);

                return await query.ToListAsync(cancellationToken);
            }
            else
            {
                return new List<int>();
            }
        }

        public async Task SetParentAsync(int parentId, int childId, string relationKey, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{GetType()}, {nameof(SetParentAsync)}, parentId : {parentId}, childId : {childId}, relationKey : {relationKey}");

            using var context = _dbContextFactory.CreateDbContext();

            if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(relationKey), out var relationEntry))
            {
                var entity = context.Find(relationEntry.ChildEntityType, childId);
                relationEntry.ParentEntityIdProperty.SetValue(entity, parentId);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
