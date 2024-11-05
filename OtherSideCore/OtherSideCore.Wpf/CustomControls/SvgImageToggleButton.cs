using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgImageToggleButton : ToggleButton
   {
      public static readonly DependencyProperty SvgImageToggleButton_ImageSizeProperty =
        DependencyProperty.Register("SvgImageToggleButton_ImageSize", typeof(int), typeof(SvgImageToggleButton), new UIPropertyMetadata(24));

      public int SvgImageToggleButton_ImageSize
      {
         get { return (int)GetValue(SvgImageToggleButton_ImageSizeProperty); }
         set { SetValue(SvgImageToggleButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_IconGeometryProperty =
        DependencyProperty.Register("SvgImageToggleButton_IconGeometry", typeof(Geometry), typeof(SvgImageToggleButton), new UIPropertyMetadata(null));

      public Geometry SvgImageToggleButton_IconGeometry
      {
         get { return (Geometry)GetValue(SvgImageToggleButton_IconGeometryProperty); }
         set { SetValue(SvgImageToggleButton_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_CheckedIconGeometryProperty =
        DependencyProperty.Register("SvgImageToggleButton_CheckedIconGeometry", typeof(Geometry), typeof(SvgImageToggleButton), new UIPropertyMetadata(null));

      public Geometry SvgImageToggleButton_CheckedIconGeometry
      {
         get
         {
            return (Geometry)GetValue(SvgImageToggleButton_CheckedIconGeometryProperty);
         }
         set { SetValue(SvgImageToggleButton_CheckedIconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_CheckedBackgroundColorProperty =
        DependencyProperty.Register("SvgImageToggleButton_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageToggleButton_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageToggleButton_CheckedBackgroundColorProperty); }
         set { SetValue(SvgImageToggleButton_CheckedBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_OverBackgroundColorProperty =
        DependencyProperty.Register("SvgImageToggleButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageToggleButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageToggleButton_OverBackgroundColorProperty); }
         set { SetValue(SvgImageToggleButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("SvgImageToggleButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageToggleButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageToggleButton_MouseDownBackgroundColorProperty); }
         set { SetValue(SvgImageToggleButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_ImageColorProperty =
        DependencyProperty.Register("SvgImageToggleButton_ImageColor", typeof(SolidColorBrush), typeof(SvgImageToggleButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgImageToggleButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageToggleButton_ImageColorProperty); }
         set { SetValue(SvgImageToggleButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageToggleButton_CornerRadiusProperty =
        DependencyProperty.Register("SvgImageToggleButton_CornerRadius", typeof(CornerRadius), typeof(SvgImageToggleButton), new UIPropertyMetadata(new CornerRadius(8)));

      public CornerRadius SvgImageToggleButton_CornerRadius
      {
         get { return (CornerRadius)GetValue(SvgImageToggleButton_ImageSizeProperty); }
         set { SetValue(SvgImageToggleButton_ImageSizeProperty, value); }
      }

      static SvgImageToggleButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgImageToggleButton), new FrameworkPropertyMetadata(typeof(SvgImageToggleButton)));
      }
   }
}
