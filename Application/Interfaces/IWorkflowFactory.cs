using Application.Workflows;
using Domain;

namespace Application.Interfaces
{
   public interface IWorkflowFactory
   {
      void RegisterWorkflow(StringKey key, Func<ProcessWorkflow> tree);
      ProcessWorkflow CreateWorkflow(StringKey key);
   }
}
