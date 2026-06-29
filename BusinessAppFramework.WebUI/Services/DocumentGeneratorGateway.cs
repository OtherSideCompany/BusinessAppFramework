using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Documents;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

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

        public async Task<DocumentDownloadResult?> DownloadPdfAsync(string key, int objectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DocumentRouteSegments.DownloadPdf}/{key}/{objectId}";
            var response = await CreateClient().GetAsync(route, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/pdf";
            var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
                ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                ?? $"{key}-{objectId}.pdf";

            return new DocumentDownloadResult(bytes, contentType, fileName);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
