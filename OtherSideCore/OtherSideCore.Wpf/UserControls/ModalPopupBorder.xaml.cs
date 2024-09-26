using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
         MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
         mainWindow.HideModal();
      }
   }
}
