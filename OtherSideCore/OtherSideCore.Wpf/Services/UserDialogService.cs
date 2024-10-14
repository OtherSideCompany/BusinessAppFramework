using OtherSideCore.Appplication.Services;
using System.Windows;

namespace OtherSideCore.Wpf.Services
{
   public class UserDialogService : IUserDialogService
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserDialogService()
      {

      }

      #endregion

      #region Public Methods

      public bool Confirm(string message)
      {
         var result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

         return result == MessageBoxResult.Yes;
      }

      public void Error(string message)
      {
         MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }

      public void Show(string message)
      {
         MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
