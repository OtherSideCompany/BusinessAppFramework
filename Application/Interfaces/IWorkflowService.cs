using Application.Workflows;

namespace Application.Interfaces
{
   public interface IWorkflowService
   {
      Task<ProcessWorkflow> GetWorkflowAsync(string workflowKey, int domainObjectId);
   }
}
