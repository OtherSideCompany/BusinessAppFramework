using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.Tree}")]
    public class TreeController : ControllerBase
    {
        #region Fields

        private ITreeFactory _treeFactory;
        private IDomainObjectServiceFactory _domainObjectServiceFactory;
        private IRelationResolver _relationResolver;
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
            IRelationResolver relationResolver,
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
            var tree = _treeFactory.CreateTree(StringKey.From(key));
            tree.RootId = domainObjectId;

            foreach (var branch in tree.Branches)
            {
                if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(branch.ParentChildRelationKey), out var parentChildRelation))
                {
                    var nodeIds = await _relationService.GetChildrenIdsAsync(domainObjectId, branch.ParentChildRelationKey);

                    var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(parentChildRelation.ChildEntityType);
                    dynamic domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService(childDomainObjectType);

                    var searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);
                    dynamic searchService = _searchServiceFactory.CreateSearchService(searchResultType);

                    foreach (var id in nodeIds)
                    {
                        var node = new Node(id);
                        node.Summary = await searchService.GetSummaryAsync(id);
                        branch.Nodes.Add(node);
                    }
                }
            }

            return tree;
        }

        [HttpPost($"{TreeRouteSegments.CreateNode}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<Node?>> CreateNode(
             [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
             [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            Node? node = null;

            if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(key), out var parentChildRelation))
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
                    await domainObjectService.SaveAsync(domainObject);
                }

                node = new Node(domainObject.Id);

                var searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);
                dynamic searchService = _searchServiceFactory.CreateSearchService(searchResultType);
                node.Summary = await searchService.GetSummaryAsync(domainObject.Id);
            }

            return Ok(node);
        }

        [HttpDelete($"{TreeRouteSegments.DeleteNode}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult<bool>> DeleteNode(
            [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId, 
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(key), out var parentChildRelation))
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



        #endregion
    }
}
