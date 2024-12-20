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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStep(string name)
      {
         Name = name;
      }

      #endregion

      #region Public Methods

      public abstract bool IsExecutable();
      public abstract bool IsCompleted();

      #endregion

      #region Private Methods



      #endregion
   }
}
