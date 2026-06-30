using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class TreeController : ControllerBase
    {
        #region Fields

        private ITreeFactory _treeFactory;
        private IDomainObjectServiceFactory _domainObjectServiceFactory;
        private IParentChildRelationResolver _relationResolver;
        private IDomainObjectTypeMap _domainObjectTypeMap;
        private IRelationService _relationService;
        private ISearchServiceFactory _searchServiceFactory;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public TreeController(
            ITreeFactory treeFactory,
            IDomainObjectServiceFactory domainObjectServiceFactory,
            IParentChildRelationResolver relationResolver,
            IDomainObjectTypeMap domainObjectTypeMap,
            IRelationService relationService,
            ISearchServiceFactory searchServiceFactory)
        {
            _treeFactory = treeFactory;
            _domainObjectServiceFactory = domainObjectServiceFactory;
            _relationResolver = relationResolver;
            _domainObjectTypeMap = domainObjectTypeMap;
            _relationService = relationService;
            _searchServiceFactory = searchServiceFactory;
        }

        #endregion

        #region Public Methods

        [HttpGet($"{TreeRouteSegments.GetTree}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<Tree>> GetTreeAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            var tree = _treeFactory.CreateTree(key);
            tree.RootId = domainObjectId;

            foreach (var branch in tree.Branches)
            {
                await LoadBranchAsync(branch, domainObjectId, 0, new HashSet<int>());
            }

            return tree;
        }

        [HttpGet($"{TreeRouteSegments.GetTreeBranch}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}/{{{ApiRouteParams.RelationKey}}}")]
        public async Task<ActionResult<Branch?>> GetTreeBranchAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string treeKey,
            [FromRoute(Name = ApiRouteParams.RelationKey)] string relationKey)
        {
            var tree = _treeFactory.CreateTree(treeKey);
            tree.RootId = domainObjectId;

            var branch = tree.GetBranch(relationKey);

            if (branch == null)
                return NoContent();

            await LoadBranchAsync(branch, domainObjectId, 0, new HashSet<int>());        

            return branch;
        }

        [HttpPost($"{TreeRouteSegments.CreateNode}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<Node?>> CreateNode(
             [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
             [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            Node? node = null;

            if (_relationResolver.TryGetParentChildRelationEntry(key, out var parentChildRelation))
            {
                var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(parentChildRelation.ChildEntityType);

                int? nextIndex = null;

                if (childDomainObjectType != null && typeof(IIndexable).IsAssignableFrom(childDomainObjectType))
                {
                    var maxIndex = await _relationService.GetMaxChildIndexAsync(parentDomainObjectId, key);
                    nextIndex = maxIndex.HasValue ? maxIndex.Value + 1 : 0;
                }

                dynamic domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService(childDomainObjectType);
                var domainObject = await domainObjectService.CreateAsync();
                await _relationService.SetParentAsync(parentDomainObjectId, domainObject.Id, key);                
                
                if (domainObject is IIndexable indexable && nextIndex.HasValue)
                {
                    indexable.Index = nextIndex.Value;
                    await domainObjectService.SaveIndexAsync(domainObject);
                }

                var searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);

                node = new Node(domainObject.Id) { TypeKey = DomainObjectSearchResultAggregateKeys.Type(searchResultType) };
                
                dynamic searchService = _searchServiceFactory.CreateSearchService(searchResultType);
                node.Summary = await searchService.GetSummaryAsync(domainObject.Id);
            }

            if (node == null)
                return NoContent();

            return Ok(node);
        }

        [HttpGet($"{TreeRouteSegments.GetNode}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<Node?>> GetNode(
             [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
             [FromRoute(Name = ApiRouteParams.DomainObjectId)] int childDomainObjectId,
             [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            Node? node = null;

            if (_relationResolver.TryGetParentChildRelationEntry(key, out var parentChildRelation))
            {
                var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(parentChildRelation.ChildEntityType);

                dynamic domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService(childDomainObjectType);
                var domainObject = await domainObjectService.GetAsync(childDomainObjectId);               

                var searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);

                node = new Node(domainObject.Id) { TypeKey = DomainObjectSearchResultAggregateKeys.Type(searchResultType) };

                dynamic searchService = _searchServiceFactory.CreateSearchService(searchResultType);
                node.Summary = await searchService.GetSummaryAsync(domainObject.Id);
            }

            if (node == null)
                return NoContent();

            return Ok(node);
        }

        [HttpDelete($"{TreeRouteSegments.DeleteNode}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<bool>> DeleteNode(
            [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId, 
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            if (_relationResolver.TryGetParentChildRelationEntry(key, out var parentChildRelation))
            {
                var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(parentChildRelation.ChildEntityType);
                dynamic domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService(childDomainObjectType);
                var domainObject = await domainObjectService.DeleteAsync(domainObjectId);
                return Ok(true);
            }

            return Ok(false);
        }

        #endregion

        #region Private Methods

        private async Task LoadBranchAsync(Branch branch, int domainObjectId, int depth, IReadOnlySet<int> ancestorIds)
        {
            if (!_relationResolver.TryGetParentChildRelationEntry(branch.ParentChildRelationKey, out var parentChildRelation))
                return;

            var nodeIds = await _relationService.GetChildrenIdsAsync(domainObjectId, branch.ParentChildRelationKey);
            var searchService = CreateSearchServiceFor(parentChildRelation.ChildEntityType, out var searchResultType);

            foreach (var id in nodeIds)
            {
                var node = await BuildNodeAsync(id, searchResultType, searchService, depth, ancestorIds);
                branch.Nodes.Add(node);

                if (node.IsCyclic)
                    continue;

                await LoadChildBranchesAsync(node, branch.ChildBranchTemplates, depth, ancestorIds);
            }
        }

        private async Task<Node> BuildNodeAsync(int id, Type searchResultType, dynamic searchService, int depth, IReadOnlySet<int> ancestorIds)
        {
            var node = new Node(id)
            {
                TypeKey = DomainObjectSearchResultAggregateKeys.Type(searchResultType),
                Depth = depth,
                IsCyclic = ancestorIds.Contains(id)
            };
            node.Summary = await searchService.GetSummaryAsync(id);
            return node;
        }

        private async Task LoadChildBranchesAsync(Node node, IReadOnlyList<Branch> templates, int depth, IReadOnlySet<int> ancestorIds)
        {
            var childAncestorIds = new HashSet<int>(ancestorIds) { node.Id };

            foreach (var template in templates)
            {
                var childBranch = new Branch(template);
                node.ChildBranches.Add(childBranch);
                await LoadBranchAsync(childBranch, node.Id, depth + 1, childAncestorIds);
            }
        }

        private dynamic CreateSearchServiceFor(Type childEntityType, out Type searchResultType)
        {
            var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(childEntityType);
            searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);
            return _searchServiceFactory.CreateSearchService(searchResultType);
        }

        #endregion
    }
}
