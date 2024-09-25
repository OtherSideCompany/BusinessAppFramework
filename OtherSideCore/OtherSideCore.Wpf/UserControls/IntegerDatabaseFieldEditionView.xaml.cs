using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
   /// Interaction logic for IntegerDatabaseFieldEditionView.xaml
   /// </summary>
   public partial class IntegerDatabaseFieldEditionView : UserControl
   {
      public IntegerDatabaseFieldEditionView()
      {
         InitializeComponent();
      }

      private void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
      {
         Regex regex = new Regex("[^0-9]+");
         e.Handled = regex.IsMatch(e.Text);
      }

      private void IntegerTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
         {
            e.Handled = false;
         }
         else
         {
            e.Handled = e.Key == Key.Space;
         }
      }
   }
}
