using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectDocumentServiceGateway<T> : HttpService, IDomainObjectDocumentServiceGateway<T> where T : DomainObject
    {
        #region Fields

        private readonly IConfiguration _configuration;

        private string _controllerKey => _domainObjectRouteKeyRegistry.GetRouteKey<T>();
        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Documents}/{_controllerKey}";

        private IDomainObjectRouteKeyRegistry _domainObjectRouteKeyRegistry;

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public DomainObjectDocumentServiceGateway(
         IHttpClientFactory clientFactory,
         IOptions<ApiClientOptions> apiClientOptions,
         IConfiguration configuration,
         IDomainObjectRouteKeyRegistry domainObjectRouteKeyRegistry) :
         base(clientFactory, apiClientOptions)
        {
            _configuration = configuration;
            _domainObjectRouteKeyRegistry = domainObjectRouteKeyRegistry;
        }

        #endregion

        #region Public Methods

        public async Task<int> UploadDocumentAsync(int domainObjectId, string relationKey, string fileName, string contentType, Stream fileStream, CancellationToken cancellationToken = default)
        {
            using var form = new MultipartFormDataContent();

            var fileContent = new StreamContent(fileStream);

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

        public string GetDownloadDocumentUrl(int documentId, CancellationToken cancellationToken = default)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var route = $"{_baseUrl}/{DocumentRouteSegments.GetDownloadUrl}/{documentId}";
            return $"{apiBaseUrl}/{route}";
        }

        #endregion

        #region Private methods



        #endregion
    }
}
