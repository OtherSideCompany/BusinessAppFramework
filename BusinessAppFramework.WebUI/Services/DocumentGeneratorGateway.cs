using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
   public class DocumentGeneratorGateway : HttpService, IDocumentGeneratorGateway
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DocumentGeneratorGateway(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
        : base(clientFactory, apiClientOptions)
      {

      }

      #endregion

      #region Public Methods

      public async Task<DocumentHtmlResponse?> GetHtmlDocumentAsync(string key, int objectId)
      {
         return (await GetAsync<DocumentHtmlResponse>(Routes.BuildRoute(Routes.GetHtmlDocumentTemplate, objectId, key))).Data;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
