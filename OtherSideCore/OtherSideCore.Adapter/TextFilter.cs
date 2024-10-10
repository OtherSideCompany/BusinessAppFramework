using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Adapter
{
   public class TextFilter : ObservableObject
   {
      #region Fields

      private string m_Text;

      #endregion

      #region Properties

      public string Text
      {
         get => m_Text;
         set => SetProperty(ref m_Text, value);
      }

      #endregion

      #region Constructor

      public TextFilter(string text)
      {
         Text = text;
      }

      #endregion

      #region Public Methods



      #endregion
   }
}
