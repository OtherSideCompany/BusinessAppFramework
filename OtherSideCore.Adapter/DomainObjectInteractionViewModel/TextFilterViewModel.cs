using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class TextFilterViewModel : ObservableObject
   {
      #region Fields

      private string _text;

      #endregion

      #region Properties

      public string Text
      {
         get => _text;
         set => SetProperty(ref _text, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public TextFilterViewModel()
      {
         Text = "Recherche...";
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
