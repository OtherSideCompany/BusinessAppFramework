using Application.Workflows;

namespace WebUI.Interfaces
{
   public interface IWorkflowServiceGateway
   {
      Task<ProcessWorkflow?> GetAsync(string workflowKey, int domainObjectId, CancellationToken cancellationToken = default);
   }
}
