using System.Windows;
using System.Windows.Controls;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for ModalPopupBorder.xaml
   /// </summary>
   public partial class ModalPopupBorder : UserControl
   {
      public ModalPopupBorder()
      {
         InitializeComponent();
      }

      private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
      {
         MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
         mainWindow.HideTopModal();
      }
   }
}
