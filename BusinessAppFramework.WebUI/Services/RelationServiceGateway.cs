using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
   public class RelationServiceGateway : HttpService, IRelationServiceGateway
   {
      #region Fields



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
         await PutAsync<object>(Routes.BuildRoute(Routes.SetParentTemplate, parentId, childId, key), null);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
