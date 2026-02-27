using BusinessAppFramework.Contracts;
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

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectDocumentServiceGateway(
         IHttpClientFactory clientFactory, 
         IOptions<ApiClientOptions> apiClientOptions,
         IConfiguration configuration) : 
         base(clientFactory, apiClientOptions)
      {
         _configuration = configuration;
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

         var response = await CreateClient().PostAsync(Routes.BuildRoute(Routes.UploadDocumentTemplate, typeof(T).Name + "Document", domainObjectId), form, cancellationToken);

         response.EnsureSuccessStatusCode();

         return await response.Content.ReadFromJsonAsync<int>(cancellationToken);
      }

      public async Task DeleteDocumentAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         var response = await CreateClient().DeleteAsync(Routes.BuildRoute(Routes.DeleteDocumentTemplate, typeof(T).Name + "Document", domainObjectId), cancellationToken);

         response.EnsureSuccessStatusCode();
      }

      public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         var response = await CreateClient().GetAsync(Routes.BuildRoute(Routes.DocumentExistsTemplate, typeof(T).Name + "Document", domainObjectId), cancellationToken);

         response.EnsureSuccessStatusCode();

         return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
      }

      public string GetDownloadDocumentUrl(int domainObjectId, CancellationToken cancellationToken = default)
      {
         var apiBaseUrl = _configuration["ApiBaseUrl"];
         var route = Routes.BuildRoute(Routes.DownloadDocumentTemplate, typeof(T).Name + "Document", domainObjectId);
         return $"{apiBaseUrl}/{route}";
      }

      #endregion

      #region Private methods



      #endregion
   }
}
