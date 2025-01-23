using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Workflows
{
   public class ProcessWorkflowStepCondition
   {
      #region Fields



      #endregion

      #region Properties

      public string Description { get; set; }
      public Func<bool> Evaluate { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStepCondition(string description, Func<bool> evaluate)
      {
         Description = description;
         Evaluate = evaluate;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
