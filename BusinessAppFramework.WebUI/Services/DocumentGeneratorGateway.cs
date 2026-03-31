using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class DocumentGeneratorGateway : HttpService, IDocumentGeneratorGateway
    {
        #region Fields
        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.DocumentGenerator}";

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DocumentGeneratorGateway(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<DocumentGeneratorGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
        : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {

        }

        #endregion

        #region Public Methods

        public async Task<DocumentHtmlResponse?> GetHtmlDocumentAsync(string key, int objectId)
        {
            var route = $"{_baseUrl}/{DocumentRouteSegments.GetHtml}/{key}/{objectId}";
            return (await GetAsync<DocumentHtmlResponse>(route)).Data;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
