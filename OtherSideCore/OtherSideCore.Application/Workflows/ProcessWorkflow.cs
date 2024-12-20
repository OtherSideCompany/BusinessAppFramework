using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Workflows
{
   public abstract class ProcessWorkflow
   {
      #region Fields



      #endregion

      #region Properties

      public List<ProcessWorkflowStep> ProcessWorkflowSteps { get; set; }
      public string Name { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflow()
      {
         ProcessWorkflowSteps = new List<ProcessWorkflowStep>();
      }

      #endregion

      #region Public Methods

      public void GoNext()
      {

      }

      public void GoBack()
      {

      }

      public void GoToStep(ProcessWorkflowStep step)
      {

      }

      #endregion

      #region Private Methods



      #endregion
   }
}
