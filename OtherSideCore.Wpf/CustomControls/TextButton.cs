using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace OtherSideCore.Wpf.CustomControls
{
   public class TextButton : Button
   {
      public static readonly DependencyProperty TextButton_OverBackgroundColorProperty =
        DependencyProperty.Register("TextButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(TextButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush TextButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(TextButton_OverBackgroundColorProperty); }
         set { SetValue(TextButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty TextButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("TextButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(TextButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush TextButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(TextButton_MouseDownBackgroundColorProperty); }
         set { SetValue(TextButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty TextButton_CornerRadiusProperty =
        DependencyProperty.Register("TextButton_CornerRadius", typeof(CornerRadius), typeof(TextButton), new UIPropertyMetadata(new CornerRadius(4)));

      public CornerRadius TextButton_CornerRadius
      {
         get { return (CornerRadius)GetValue(TextButton_CornerRadiusProperty); }
         set { SetValue(TextButton_CornerRadiusProperty, value); }
      }

      static TextButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(TextButton), new FrameworkPropertyMetadata(typeof(TextButton)));
      }
   }
}
