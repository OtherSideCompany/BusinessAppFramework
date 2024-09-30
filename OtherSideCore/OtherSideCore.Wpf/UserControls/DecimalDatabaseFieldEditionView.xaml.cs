using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Globalization;
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
   /// Interaction logic for DecimalDatabaseFieldEditionView.xaml
   /// </summary>
   public partial class DecimalDatabaseFieldEditionView : UserControl
   {
      public DecimalDatabaseFieldEditionView()
      {
         InitializeComponent();
      }

      private void DecimalTextBox_LostFocus(object sender, RoutedEventArgs e)
      {
         ((DecimalDatabaseField)DataContext).LoadBuffer();
      }
   }
}
