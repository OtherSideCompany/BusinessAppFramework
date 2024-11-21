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
   public abstract class ViewBase
   {
      #region Fields

      protected IUserContext _userContext;
      protected ILoggerFactory _loggerFactory;
      protected IUserDialogService _userDialogService;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewBase(ILoggerFactory loggerFactory,
                      IUserContext userContext,
                      IUserDialogService userDialogService)
      {
         _userContext = userContext;
         _loggerFactory = loggerFactory;
         _userDialogService = userDialogService;
      }

      #endregion

      #region Public Methods

      public abstract Task InitializeAsync();

      public abstract void Dispose();

      #endregion

      #region Private Methods



      #endregion
   }
}
