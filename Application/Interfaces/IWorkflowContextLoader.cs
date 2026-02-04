using Application.Workflows;

namespace Application.Interfaces
{
   public interface IWorkflowContextLoader
   {
      Task<WorkflowContext> LoadAsync(int domainObjectId, CancellationToken cancellationToken = default);
   }
}
