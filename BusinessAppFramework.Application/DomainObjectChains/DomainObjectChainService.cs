using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BusinessAppFramework.Application.DomainObjectChains
{
    public class DomainObjectChainService : IDomainObjectChainService
    {
        #region Fields

        private const int MaxDepth = 20;

        private readonly IDomainObjectChainRegistry _registry;
        private readonly IRelationService _relationService;
        private readonly IParentChildRelationResolver _parentChildRelationResolver;
        private readonly IDomainObjectTypeMap _domainObjectTypeMap;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserPermissionResolverService _userPermissionResolverService;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Constructor

        public DomainObjectChainService(
            IDomainObjectChainRegistry registry,
            IRelationService relationService,
            IParentChildRelationResolver parentChildRelationResolver,
            IDomainObjectTypeMap domainObjectTypeMap,
            IServiceProvider serviceProvider,
            IUserPermissionResolverService userPermissionResolverService,
            ICurrentUserService currentUserService)
        {
            _registry = registry;
            _relationService = relationService;
            _parentChildRelationResolver = parentChildRelationResolver;
            _domainObjectTypeMap = domainObjectTypeMap;
            _serviceProvider = serviceProvider;
            _userPermissionResolverService = userPermissionResolverService;
            _currentUserService = currentUserService;
        }

        #endregion

        #region Public Methods

        public async Task<DomainObjectChain> GetChainAsync(Type domainObjectType, int domainObjectId, DomainObjectChainDirection direction, CancellationToken ct = default)
        {
            var anchorType = _registry.ResolveType(domainObjectType) ?? domainObjectType;
            var anchor = (Type: anchorType, Id: domainObjectId);

            var ancestors = new Dictionary<(Type Type, int Id), int>();
            var descendants = new Dictionary<(Type Type, int Id), int>();

            if (direction is DomainObjectChainDirection.Ancestors or DomainObjectChainDirection.Both)
                await CollectAsync(anchor, anchor, upstream: true, ancestors, 0, ct);

            if (direction is DomainObjectChainDirection.Descendants or DomainObjectChainDirection.Both)
                await CollectAsync(anchor, anchor, upstream: false, descendants, 0, ct);

            var ordered = ancestors.OrderByDescending(a => a.Value).ThenBy(a => TypeKey(a.Key.Type)).ThenBy(a => a.Key.Id).Select(a => a.Key)
                .Append(anchor)
                .Concat(descendants.OrderBy(d => d.Value).ThenBy(d => TypeKey(d.Key.Type)).ThenBy(d => d.Key.Id).Select(d => d.Key));

            var chain = new DomainObjectChain();
            var canAccessByType = new Dictionary<Type, bool>();

            foreach (var (type, id) in ordered)
            {
                if (!canAccessByType.TryGetValue(type, out var canAccess))
                {
                    canAccess = _currentUserService.UserId != null
                        && await _userPermissionResolverService.CanAccessAsync(DomainObjectAggregateKeys.PermissionKey(type), _currentUserService.UserId.Value);

                    canAccessByType[type] = canAccess;
                }

                chain.Nodes.Add(new DomainObjectChainNode
                {
                    DomainObjectTypeKey = TypeKey(type),
                    DomainObjectId = id,
                    LabelKey = DomainObjectAggregateKeys.PageWorkspace(type),
                    IconKey = DomainObjectAggregateKeys.Workspace(type),
                    ReferenceNumber = (await LoadAsync(type, id, ct))?.ToString() ?? string.Empty,
                    IsCurrent = (type, id) == anchor,
                    CanAccess = canAccess
                });
            }

            return chain;
        }

        #endregion

        #region Private Methods

        private static string TypeKey(Type type) => DomainObjectAggregateKeys.Type(type);

        private async Task CollectAsync(
            (Type Type, int Id) from,
            (Type Type, int Id) anchor,
            bool upstream,
            Dictionary<(Type Type, int Id), int> depths,
            int depth,
            CancellationToken ct)
        {
            if (depth >= MaxDepth)
                return;

            foreach (var link in upstream ? _registry.GetLinksTo(from.Type) : _registry.GetLinksFrom(from.Type))
            {
                var reachedType = upstream ? link.FromType : link.ToType;

                foreach (var reachedId in await FollowAsync(from, link.RelationKey, ct))
                {
                    var reached = (Type: reachedType, Id: reachedId);

                    if (reached == anchor)
                        continue;

                    if (depths.TryGetValue(reached, out var known) && known >= depth + 1)
                        continue;

                    depths[reached] = depth + 1;

                    await CollectAsync(reached, anchor, upstream, depths, depth + 1, ct);
                }
            }
        }

        private async Task<List<int>> FollowAsync((Type Type, int Id) from, string relationKey, CancellationToken ct)
        {
            if (!_parentChildRelationResolver.TryGetParentChildRelationEntry(relationKey, out var relation))
                return new List<int>();

            if (_domainObjectTypeMap.GetDomainTypeFromEntityType(relation.ParentEntityType) == from.Type)
                return await _relationService.GetChildrenIdsAsync(from.Id, relationKey, ct);

            var parentId = await _relationService.GetParentIdAsync(from.Id, relationKey, ct);

            return parentId == null ? new List<int>() : new List<int> { parentId.Value };
        }

        private async Task<DomainObject?> LoadAsync(Type type, int id, CancellationToken ct)
        {
            var method = typeof(DomainObjectChainService).GetMethod(nameof(LoadTypedAsync), BindingFlags.NonPublic | BindingFlags.Instance)!.MakeGenericMethod(type);

            return await (Task<DomainObject?>)method.Invoke(this, new object[] { id, ct })!;
        }

        private async Task<DomainObject?> LoadTypedAsync<T>(int id, CancellationToken ct) where T : DomainObject, new()
        {
            return await _serviceProvider.GetRequiredService<IRepository<T>>().GetAsync(id, ct);
        }

        #endregion
    }
}
