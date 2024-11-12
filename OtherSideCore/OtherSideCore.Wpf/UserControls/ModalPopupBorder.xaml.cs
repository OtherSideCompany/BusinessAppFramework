using OtherSideCore.Adapter.Views;
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
         MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext;
         mainWindowViewModel.WindowService.HideTopModal();
      }
   }
}
