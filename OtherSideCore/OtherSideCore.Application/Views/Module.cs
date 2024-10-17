using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Views
{
   public abstract class Module : ViewBase
   {
      #region Fields

      

      #endregion

      #region Properties

      

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Module(ILoggerFactory loggerFactory,
                    IUserContext userContext,
                    IUserDialogService userDialogService) :
         base(loggerFactory,
              userContext,
              userDialogService)
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
