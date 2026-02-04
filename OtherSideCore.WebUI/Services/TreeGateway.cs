using Microsoft.Extensions.Options;
using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Application.Trees;
using OtherSideCore.Contracts;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.WebUI.Interfaces;
using System.Text.Json;

namespace OtherSideCore.WebUI.Services
{
    public class TreeGateway : HttpService, ITreeGateway
    {
        #region Fields

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
            IDomainObjectTypeMap domainObjectTypeMap) : base(clientFactory, apiClientOptions)
        {
            _relationResolver = relationResolver;
            _domainObjectTypeMap = domainObjectTypeMap;
        }

        #endregion

        #region Public Methods

        public async Task<Tree?> GetTreeAsync(int domainObjectId, string key)
        {
            var tree = (await GetAsync<Tree>(Routes.BuildRoute(Routes.GetTreeTemplate, domainObjectId, key))).Data;

            return tree;
        }

        public async Task<Node?> CreateNode(int parentDomainObjectId, string parentChildRelationKey)
        {
            return (await PostAsync<Node>(Routes.BuildRouteFromParent(Routes.CreateTreeNodeTemplate, parentDomainObjectId, parentChildRelationKey), null)).Data;
        }

        public async Task<bool> DeleteNodeAsync(int parentId, int childId, string parentChildRelationKey)
        {
            var result = await DeleteAsync<bool>(Routes.BuildRoute(Routes.DeleteTreeNodeTemplate, parentId, childId, parentChildRelationKey));

            return result?.Data == true;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
