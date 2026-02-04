using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
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

      [HttpGet(Routes.GetTreeTemplate)]
      public async Task<ActionResult<Tree>> GetTreeAsync(int domainObjectId, string key)
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

      [HttpPost(Routes.CreateTreeNodeTemplate)]
      public async Task<ActionResult<Node?>> CreateNode(int parentDomainObjectId, string key)
      {
         Node? node = null;

         if (_relationResolver.TryGetParentChildRelationEntry(StringKey.From(key), out var parentChildRelation))
         {
            var childDomainObjectType = _domainObjectTypeMap.GetDomainTypeFromEntityType(parentChildRelation.ChildEntityType);
            dynamic domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService(childDomainObjectType);
            var domainObject = await domainObjectService.CreateAsync();
            await _relationService.SetParentAsync(parentDomainObjectId, domainObject.Id, key);
            node = new Node(domainObject.Id);

            var searchResultType = _domainObjectTypeMap.GetSearchResultTypeFromDomainType(childDomainObjectType);
            dynamic searchService = _searchServiceFactory.CreateSearchService(searchResultType);
            node.Summary = await searchService.GetSummaryAsync(domainObject.Id);
         }

         return Ok(node);
      }

      [HttpDelete(Routes.DeleteTreeNodeTemplate)]
      public async Task<ActionResult<bool>> DeleteNode(int parentDomainObjectId, int domainObjectId, string key)
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
