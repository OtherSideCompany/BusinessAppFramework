
using Contracts;
using Microsoft.Extensions.Options;
using WebUI.Interfaces;

namespace WebUI.Services
{
   public class WorkspaceNavigationService : HttpService, IWorkspaceNavigationService
   {
      #region Fields



      #endregion

      #region Constructor

      public WorkspaceNavigationService(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
          : base(clientFactory, apiClientOptions)
      {

      }

      #endregion

      #region Public Methods

      public async Task<List<string>> GetModuleKeysAsync()
      {
         return (await GetAsync<List<string>>(Routes.ModulesTemplate)).Data ?? new List<string>();
      }

      public async Task<List<string>> GetWorkspaceKeysAsync(string moduleWorkspaceKey)
      {
         return (await GetAsync<List<string>>(Routes.WorkspacesFor(moduleWorkspaceKey))).Data ?? new List<string>();
      }

      #endregion

      #region Private Methods


      #endregion
   }
}
