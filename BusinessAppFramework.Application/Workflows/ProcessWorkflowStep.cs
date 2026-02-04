namespace BusinessAppFramework.Application.Workflows
{
   public class ProcessWorkflowStep
   {
      #region Fields



      #endregion

      #region Properties

      public string Key { get; set; }
      public bool IsCompleted => ProcessWorkflowStepConditions.Any() && ProcessWorkflowStepConditions.All(s => s.IsCompleted);
      public List<ProcessWorkflowStepCondition> ProcessWorkflowStepConditions { get; set; } = new();

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStep(string key)
      {
         Key = key;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
