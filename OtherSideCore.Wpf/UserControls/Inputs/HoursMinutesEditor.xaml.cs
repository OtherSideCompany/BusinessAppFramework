using OtherSideCore.Wpf.UserControls;
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

namespace OtherSideCore.Wpf.UserControls.Input
{
   /// <summary>
   /// Interaction logic for HoursMinutesEditor.xaml
   /// </summary>
   public partial class HoursMinutesEditor : UserControl
   {
      public static readonly DependencyProperty HoursMinutesEditor_HoursProperty =
          DependencyProperty.Register("HoursMinutesEditor_Hours", typeof(decimal), typeof(HoursMinutesEditor), new UIPropertyMetadata(0m));

      public decimal HoursMinutesEditor_Hours
      {
         get { return (decimal)GetValue(HoursMinutesEditor_HoursProperty); }
         set { SetValue(HoursMinutesEditor_HoursProperty, value); }
      }

      public static readonly DependencyProperty HoursMinutesEditor_MinutesProperty =
          DependencyProperty.Register("HoursMinutesEditor_Minutes", typeof(decimal), typeof(HoursMinutesEditor), new UIPropertyMetadata(0m));

      public decimal HoursMinutesEditor_Minutes
      {
         get { return (decimal)GetValue(HoursMinutesEditor_MinutesProperty); }
         set { SetValue(HoursMinutesEditor_MinutesProperty, value); }
      }


      public HoursMinutesEditor()
      {
         InitializeComponent();
      }
   }
}
