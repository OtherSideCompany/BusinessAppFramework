using BusinessAppFramework.Application.Workflows;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IWorkflowServiceGateway
   {
      Task<ProcessWorkflow?> GetAsync(string workflowKey, int domainObjectId, CancellationToken cancellationToken = default);
   }
}
