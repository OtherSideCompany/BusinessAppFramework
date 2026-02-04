using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Presentation
{
   public class PresentationWorkspace : IPresentationWorkspace
   {
      #region Fields



      #endregion

      #region Properties

      public PresentationDescription PresentationDescription { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public PresentationWorkspace()
      {
         PresentationDescription = new PresentationDescription();
      }      

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
