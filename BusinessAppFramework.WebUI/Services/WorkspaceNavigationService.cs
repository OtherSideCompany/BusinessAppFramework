
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class WorkspaceNavigationService : HttpService, IWorkspaceNavigationService
    {
        #region Fields

        private const string _baseUrl = $"{ApiRouteSegments.Root}/{ApiRouteSegments.Navigation}";

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
            var route = $"{_baseUrl}/{NavigationRouteSegments.Modules}";
            return (await GetAsync<List<string>>(route)).Data ?? new List<string>(); 
        }

        public async Task<List<string>> GetWorkspaceKeysAsync(string moduleKey)
        {
            var route = $"{_baseUrl}/{moduleKey}/{NavigationRouteSegments.Workspaces}";
            return (await GetAsync<List<string>>(route)).Data ?? new List<string>();
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
