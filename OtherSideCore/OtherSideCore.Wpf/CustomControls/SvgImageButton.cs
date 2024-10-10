using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgImageButton : Button
   {
      public static readonly DependencyProperty SvgImageButton_ImageSizeProperty =
        DependencyProperty.Register("SvgImageButton_ImageSize", typeof(int), typeof(SvgImageButton), new UIPropertyMetadata(24));

      public int SvgImageButton_ImageSize
      {
         get { return (int)GetValue(SvgImageButton_ImageSizeProperty); }
         set { SetValue(SvgImageButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty SvgImageButton_IconGeometryProperty =
        DependencyProperty.Register("SvgImageButton_IconGeometry", typeof(Geometry), typeof(SvgImageButton), new UIPropertyMetadata(null));

      public Geometry SvgImageButton_IconGeometry
      {
         get { return (Geometry)GetValue(SvgImageButton_IconGeometryProperty); }
         set { SetValue(SvgImageButton_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageButton_OverBackgroundColorProperty =
        DependencyProperty.Register("SvgImageButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageButton_OverBackgroundColorProperty); }
         set { SetValue(SvgImageButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("SvgImageButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageButton_MouseDownBackgroundColorProperty); }
         set { SetValue(SvgImageButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageButton_ImageColorProperty =
        DependencyProperty.Register("SvgImageButton_ImageColor", typeof(SolidColorBrush), typeof(SvgImageButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgImageButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageButton_ImageColorProperty); }
         set { SetValue(SvgImageButton_ImageColorProperty, value); }
      }

      static SvgImageButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgImageButton), new FrameworkPropertyMetadata(typeof(SvgImageButton)));
      }
   }
}
