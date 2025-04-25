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

namespace OtherSideCore.Wpf.CustomControls
{
   public class LabeledInput : ContentControl
   {
      public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledInput), new PropertyMetadata(string.Empty));

      public string Label
      {
         get => (string)GetValue(LabelProperty);
         set => SetValue(LabelProperty, value);
      }

      public static readonly DependencyProperty OrientationProperty =
         DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(LabeledInput), new PropertyMetadata(Orientation.Vertical));

      public Orientation Orientation
      {
         get => (Orientation)GetValue(OrientationProperty);
         set => SetValue(OrientationProperty, value);
      }

      static LabeledInput()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledInput), new FrameworkPropertyMetadata(typeof(LabeledInput)));
      }
   }
}
