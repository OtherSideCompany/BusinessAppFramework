using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter
{
   public class WindowSession : IWindowSession
   {
      #region Fields

      private readonly TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
      private bool _closed = false;

      #endregion

      #region Properties

      public Task WhenClosed => _tcs.Task;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowSession()
      {

      }

      #endregion

      #region Public Methods

      public void NotifyClosed()
      {
         if (_closed) return;

         _closed = true;

         _tcs.TrySetResult();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
