using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowContextLoaderFactory
   {
      void RegisterWorkflowContextLoader(string workflowKey, Func<IWorkflowContextLoader> loader);
      IWorkflowContextLoader Get(string workflowKey);
   }
}
