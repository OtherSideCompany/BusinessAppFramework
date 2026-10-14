using BusinessAppFramework.Application.DomainObjectChains;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectChainGateway : HttpService, IDomainObjectChainGateway
    {
        #region Fields

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.DomainObjectChains}";

        #endregion

        #region Constructor

        public DomainObjectChainGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<DomainObjectChainGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService) :
            base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
        }

        #endregion

        #region Public Methods

        public async Task<DomainObjectChain> GetChainAsync(string domainObjectTypeKey, int domainObjectId, DomainObjectChainDirection direction, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{ApiRouteSegments.GetChain}/{domainObjectId}?typeKey={Uri.EscapeDataString(domainObjectTypeKey)}&direction={direction}";

            return (await GetAsync<DomainObjectChain>(route)).Data ?? new();
        }

        #endregion
    }
}
