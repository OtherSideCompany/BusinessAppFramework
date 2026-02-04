using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
   public class WorkflowServiceGateway : HttpService, IWorkflowServiceGateway
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public WorkflowServiceGateway(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
          : base(clientFactory, apiClientOptions)
      {

      }

      #endregion

      #region Public Methods

      public async Task<ProcessWorkflow?> GetAsync(string workflowKey, int domainObjectId, CancellationToken cancellationToken = default)
      {
         return (await GetAsync<ProcessWorkflow>(Routes.BuildRoute(Routes.GetWorkflowTemplate, domainObjectId, workflowKey))).Data;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
