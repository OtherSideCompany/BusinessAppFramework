using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Workflows
{
   public abstract class ProcessWorkflowStep
   {
      #region Fields



      #endregion

      #region Properties

      public string Name { get; set; }
      public List<ProcessWorkflowStepCondition> ExecutableConditions { get; private set; }
      public List<ProcessWorkflowStepCondition> CompletionConditions { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStep(string name)
      {
         Name = name;
         ExecutableConditions = new List<ProcessWorkflowStepCondition>();
         CompletionConditions = new List<ProcessWorkflowStepCondition>();
      }

      #endregion

      #region Public Methods

      public bool IsExecutable() 
      {
         return ExecutableConditions.Any() ? ExecutableConditions.All(c => c.Evaluate()) : true;
      }

      public virtual bool IsCompleted()
      {
         return CompletionConditions.Any() ? CompletionConditions.All(c => c.Evaluate()) : true;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
