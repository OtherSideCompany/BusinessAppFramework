namespace BusinessAppFramework.Application.Workflows
{
   public class ProcessWorkflow
   {
      #region Fields



      #endregion

      #region Properties

      public List<ProcessWorkflowStep> ProcessWorkflowSteps { get; set; } = new();

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflow()
      {

      }

      #endregion

      #region Public Methods      

      public virtual void EvaluateWorkflowSate(WorkflowContext workflowContext)
      {

      }

      #endregion

      #region Private Methods



      #endregion
   }
}
