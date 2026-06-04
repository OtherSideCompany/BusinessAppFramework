using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Factories
{
   public class WorkflowContextLoaderFactory : stringBasedFactory, IWorkflowContextLoaderFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public WorkflowContextLoaderFactory()
      {

      }

      #endregion

      #region Public Methods

      public IWorkflowContextLoader Get(string workflowKey)
      {
         return (IWorkflowContextLoader)Create(workflowKey);
      }

      public void RegisterWorkflowContextLoader(string workflowKey, Func<IWorkflowContextLoader> loader)
      {
         Register(workflowKey, loader);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
