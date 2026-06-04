using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowFactory
   {
      void RegisterWorkflow(string key, Func<ProcessWorkflow> tree);
      ProcessWorkflow CreateWorkflow(string key);
   }
}
