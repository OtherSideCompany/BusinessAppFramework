using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Documents;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BusinessAppFramework.WebUI.Services
{
    public class DocumentServiceGateway : HttpService, IDocumentServiceGateway
    {
        #region Fields

        private readonly IConfiguration _configuration;

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Documents}";

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public DocumentServiceGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            IConfiguration configuration,
            IDomainObjectRouteKeyRegistry domainObjectRouteKeyRegistry,
            ILogger<DocumentServiceGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService) :
         base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            _configuration = configuration;
        }

        #endregion

        #region Public Methods

        public async Task<int> UploadDocumentAsync(int domainObjectId, string relationKey, string fileName, string contentType, Stream fileStream, CancellationToken cancellationToken = default)
        {
            using var form = new MultipartFormDataContent();

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            var fileContent = new ByteArrayContent(memoryStream.ToArray());

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType);

            form.Add(new StringContent(relationKey), "relationKey");
            form.Add(fileContent, "file", fileName);

            var route = $"{_baseUrl}/{DocumentRouteSegments.Upload}/{domainObjectId}";
            var response = await CreateClient().PostAsync(route, form, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        }

        public async Task DeleteDocumentAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DocumentRouteSegments.Delete}/{domainObjectId}";
            var response = await CreateClient().DeleteAsync(route, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DocumentRouteSegments.Exists}/{domainObjectId}";
            var response = await CreateClient().GetAsync(route, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
        }

        public async Task<DocumentDownloadResult?> DownloadDocumentAsync(int documentId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DocumentRouteSegments.Download}/{documentId}";
            var response = await CreateClient().GetAsync(route, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
                ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                ?? $"document-{documentId}";

            return new DocumentDownloadResult(bytes, contentType, fileName);
        }

        #endregion

        #region Private methods



        #endregion
    }
}
