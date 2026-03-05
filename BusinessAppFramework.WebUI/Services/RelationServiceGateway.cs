using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class RelationServiceGateway : HttpService, IRelationServiceGateway
    {
        #region Fields

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.RelationShips}";

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public RelationServiceGateway(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
          : base(clientFactory, apiClientOptions)
        {

        }

        #endregion

        #region Public Methods

        public async Task SetParentAsync(int parentId, int childId, string key)
        {
            var route = $"{_baseUrl}/{RelationshipSegments.SetParent}/{parentId}/{childId}/{key}";
            await PutAsync<object>(route, null);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
