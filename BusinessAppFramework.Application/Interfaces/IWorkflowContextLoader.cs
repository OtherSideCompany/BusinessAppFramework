using BusinessAppFramework.Application.Workflows;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowContextLoader
   {
      Task<WorkflowContext> LoadAsync(int domainObjectId, CancellationToken cancellationToken = default);
   }
}
