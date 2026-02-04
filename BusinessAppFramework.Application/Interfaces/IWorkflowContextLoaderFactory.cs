using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IWorkflowContextLoaderFactory
   {
      void RegisterWorkflowContextLoader(StringKey workflowKey, Func<IWorkflowContextLoader> loader);
      IWorkflowContextLoader Get(StringKey workflowKey);
   }
}
