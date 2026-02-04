using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Factories
{
   public class WorkflowFactory : StringKeyBasedFactory, IWorkflowFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public WorkflowFactory()
      {

      }

      #endregion

      #region Public Methods

      public ProcessWorkflow CreateWorkflow(StringKey key)
      {
         return (ProcessWorkflow)Create(key);
      }

      public void RegisterWorkflow(StringKey key, Func<ProcessWorkflow> tree)
      {
         Register(key, tree);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
