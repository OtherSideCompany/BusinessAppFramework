using System.Windows;
using System.Windows.Controls;

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
