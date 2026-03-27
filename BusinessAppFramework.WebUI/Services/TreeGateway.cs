using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Services;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp.Input;

namespace BusinessAppFramework.WebUI.Services
{
    public class TreeGateway : HttpService, ITreeGateway
    {
        #region Fields

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Tree}";

        private IRelationResolver _relationResolver;
        private IDomainObjectTypeMap _domainObjectTypeMap;
        
        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public TreeGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            IRelationResolver relationResolver,
            IDomainObjectTypeMap domainObjectTypeMap,
            ILogger<TreeGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService) : 
            base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            _relationResolver = relationResolver;
            _domainObjectTypeMap = domainObjectTypeMap;
        }

        #endregion

        #region Public Methods

        public async Task<Tree?> GetTreeAsync(int domainObjectId, string key)
        {
            var route = $"{_baseUrl}/{TreeRouteSegments.GetTree}/{domainObjectId}/{key}";
            var tree = (await GetAsync<Tree>(route)).Data;

            return tree;
        }

        public async Task<Branch?> GetTreeBranchAsync(int domainObjectId, string pageTreeKey, string relationKey)
        {
            var route = $"{_baseUrl}/{TreeRouteSegments.GetTreeBranch}/{domainObjectId}/{pageTreeKey}/{relationKey}";
            var branch = (await GetAsync<Branch>(route)).Data;

            return branch;
        }

        public async Task<Node?> CreateNode(int parentDomainObjectId, string parentChildRelationKey)
        {
            var route = $"{_baseUrl}/{TreeRouteSegments.CreateNode}/{parentDomainObjectId}/{parentChildRelationKey}";
            return (await PostAsync<Node>(route, null)).Data;
        }

        public async Task<bool> DeleteNodeAsync(int parentId, int childId, string parentChildRelationKey)
        {
            var route = $"{_baseUrl}/{TreeRouteSegments.DeleteNode}/{parentId}/{childId}/{parentChildRelationKey}";
            var result = await DeleteAsync<bool>(route);

            return result?.Data == true;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
