using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public class TextFilter : ObservableObject
   {
      #region Fields

      private string m_Text;

      #endregion

      #region Properties

      public string Text
      {
         get
         {
            return m_Text;
         }
         set
         {
            if (value != m_Text)
            {
               m_Text = value;
               OnPropertyChanged(nameof(Text));
            }
         }
      }

      #endregion

      #region Constructor

      public TextFilter(string text)
      {
         Text = text;
      }

      #endregion

      #region Methods



      #endregion
   }
}
