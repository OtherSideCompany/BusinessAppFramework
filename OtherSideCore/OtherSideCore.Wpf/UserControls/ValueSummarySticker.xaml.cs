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
   /// Interaction logic for ValueSummarySticker.xaml
   /// </summary>
   public partial class ValueSummarySticker : UserControl
   {
      public static readonly DependencyProperty ValueSummarySticker_IconGeometryProperty =
          DependencyProperty.Register("ValueSummarySticker_IconGeometry", typeof(Geometry), typeof(ValueSummarySticker), new UIPropertyMetadata());

      public Geometry ValueSummarySticker_IconGeometry
      {
         get { return (Geometry)GetValue(ValueSummarySticker_IconGeometryProperty); }
         set { SetValue(ValueSummarySticker_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty ValueSummarySticker_IconColorProperty =
          DependencyProperty.Register("ValueSummarySticker_IconColor", typeof(SolidColorBrush), typeof(ValueSummarySticker), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush ValueSummarySticker_IconColor
      {
         get { return (SolidColorBrush)GetValue(ValueSummarySticker_IconColorProperty); }
         set { SetValue(ValueSummarySticker_IconColorProperty, value); }
      }

      public static readonly DependencyProperty ValueSummarySticker_TextProperty =
          DependencyProperty.Register("ValueSummarySticker_Text", typeof(string), typeof(ValueSummarySticker), new UIPropertyMetadata("Kpi description"));

      public string ValueSummarySticker_Text
      {
         get { return (string)GetValue(ValueSummarySticker_TextProperty); }
         set { SetValue(ValueSummarySticker_TextProperty, value); }
      }

      public static readonly DependencyProperty ValueSummarySticker_ValueProperty =
          DependencyProperty.Register("ValueSummarySticker_Value", typeof(string), typeof(ValueSummarySticker), new UIPropertyMetadata("Kpi value"));

      public string ValueSummarySticker_Value
      {
         get { return (string)GetValue(ValueSummarySticker_ValueProperty); }
         set { SetValue(ValueSummarySticker_ValueProperty, value); }
      }

      public static readonly DependencyProperty ValueSummarySticker_ValueTextColorProperty =
          DependencyProperty.Register("ValueSummarySticker_ValueTextColor", typeof(SolidColorBrush), typeof(ValueSummarySticker), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush ValueSummarySticker_ValueTextColor
      {
         get { return (SolidColorBrush)GetValue(ValueSummarySticker_ValueTextColorProperty); }
         set { SetValue(ValueSummarySticker_ValueTextColorProperty, value); }
      }

      public ValueSummarySticker()
      {
         InitializeComponent();
      }
   }
}
