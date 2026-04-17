using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
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

        public RelationServiceGateway(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<RelationServiceGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
          : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {

        }

        #endregion

        #region Public Methods

        public async Task SetParentAsync(int parentId, int childId, string key)
        {
            var route = $"{_baseUrl}/{RelationshipSegments.SetParent}/{parentId}/{childId}/{key}";
            await PutAsync<object>(route, null);
        }

        public async Task<DomainObjectReference?> GetHydratedReferenceAsync(int parentId, int childId, string key)
        {
            var route = $"{_baseUrl}/{RelationshipSegments.GetHydratedReference}/{parentId}/{childId}/{key}";
            return (await GetAsync<DomainObjectReference?>(route)).Data;
        }

        public async Task<List<int>> GetChildrenIdsAsync(int parentId, string key)
        {
            var route = $"{_baseUrl}/{RelationshipSegments.GetChildrenIds}/{parentId}/{key}";
            return (await GetAsync<List<int>>(route)).Data;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
