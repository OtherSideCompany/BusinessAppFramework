using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgImageTextToggleButton : ToggleButton
   {
      public static readonly DependencyProperty SvgImageTextToggleButton_TextProperty =
         DependencyProperty.Register("SvgImageTextToggleButton_Text", typeof(string), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(""));

      public string SvgImageTextToggleButton_Text
      {
         get { return (string)GetValue(SvgImageTextToggleButton_TextProperty); }
         set { SetValue(SvgImageTextToggleButton_TextProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_TextMarginProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_TextMargin", typeof(Thickness), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(new Thickness(4, 0, 4, 0)));

      public Thickness SvgImageTextToggleButton_TextMargin
      {
         get { return (Thickness)GetValue(SvgImageTextToggleButton_TextMarginProperty); }
         set { SetValue(SvgImageTextToggleButton_TextMarginProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_ImageSizeProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_ImageSize", typeof(int), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(20));

      public int SvgImageTextToggleButton_ImageSize
      {
         get { return (int)GetValue(SvgImageTextToggleButton_ImageSizeProperty); }
         set { SetValue(SvgImageTextToggleButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_IconGeometryProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_IconGeometry", typeof(Geometry), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(null));

      public Geometry SvgImageTextToggleButton_IconGeometry
      {
         get { return (Geometry)GetValue(SvgImageTextToggleButton_IconGeometryProperty); }
         set { SetValue(SvgImageTextToggleButton_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_CheckedIconGeometryProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_CheckedIconGeometry", typeof(Geometry), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(null));

      public Geometry SvgImageTextToggleButton_CheckedIconGeometry
      {
         get
         {
            return (Geometry)GetValue(SvgImageTextToggleButton_CheckedIconGeometryProperty);
         }
         set { SetValue(SvgImageTextToggleButton_CheckedIconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_CheckedBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextToggleButton_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextToggleButton_CheckedBackgroundColorProperty); }
         set { SetValue(SvgImageTextToggleButton_CheckedBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_OverBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextToggleButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextToggleButton_OverBackgroundColorProperty); }
         set { SetValue(SvgImageTextToggleButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextToggleButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextToggleButton_MouseDownBackgroundColorProperty); }
         set { SetValue(SvgImageTextToggleButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_ImageColorProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_ImageColor", typeof(SolidColorBrush), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgImageTextToggleButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextToggleButton_ImageColorProperty); }
         set { SetValue(SvgImageTextToggleButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextToggleButton_OrientationProperty =
        DependencyProperty.Register("SvgImageTextToggleButton_Orientation", typeof(OrientationType), typeof(SvgImageTextToggleButton), new UIPropertyMetadata(OrientationType.Horizontal));

      public OrientationType SvgImageTextToggleButton_Orientation
      {
         get { return (OrientationType)GetValue(SvgImageTextToggleButton_OrientationProperty); }
         set { SetValue(SvgImageTextToggleButton_OrientationProperty, value); }
      }

      static SvgImageTextToggleButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgImageTextToggleButton), new FrameworkPropertyMetadata(typeof(SvgImageTextToggleButton)));
      }
   }
}
