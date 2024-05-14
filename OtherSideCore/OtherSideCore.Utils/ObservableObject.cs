using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Utils
{
   public class ObservableObject
   {
      #region Fields

      public event PropertyChangedEventHandler PropertyChanged;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ObservableObject()
      {

      }

      #endregion

      #region Methods

      protected void OnPropertyChanged(string name)
      {
         PropertyChangedEventHandler handler = PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(name));
         }
      }

      #endregion
   }
}
