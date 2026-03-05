using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Services;
using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class WorkflowServiceGateway : HttpService, IWorkflowServiceGateway
    {
        #region Fields

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Workflow}";

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowServiceGateway(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions,
            ILocalizedStringService localizedStringService,
            ILogger<WorkflowServiceGateway> logger,
            IUserDialogService userDialogService)
          : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {

        }

        #endregion

        #region Public Methods

        public async Task<ProcessWorkflow?> GetAsync(string workflowKey, int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{WorkflowRouteSegments.GetWorkflow}/{workflowKey}/{domainObjectId}";
            return (await GetAsync<ProcessWorkflow>(route)).Data;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
