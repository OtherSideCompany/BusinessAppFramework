using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowFactory
   {
      void RegisterWorkflow(StringKey key, Func<ProcessWorkflow> tree);
      ProcessWorkflow CreateWorkflow(StringKey key);
   }
}
