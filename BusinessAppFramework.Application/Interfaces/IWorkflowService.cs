using BusinessAppFramework.Application.Workflows;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowService
   {
      Task<ProcessWorkflow> GetWorkflowAsync(string workflowKey, int domainObjectId);
   }
}
