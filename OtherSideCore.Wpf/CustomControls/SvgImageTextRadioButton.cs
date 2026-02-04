using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgImageTextRadioButton : RadioButton
   {
      public static readonly DependencyProperty SvgImageTextRadioButton_TextProperty =
         DependencyProperty.Register("SvgImageTextRadioButton_Text", typeof(string), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(""));

      public string SvgImageTextRadioButton_Text
      {
         get { return (string)GetValue(SvgImageTextRadioButton_TextProperty); }
         set { SetValue(SvgImageTextRadioButton_TextProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_TextMarginProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_TextMargin", typeof(Thickness), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(new Thickness(4, 0, 4, 0)));

      public Thickness SvgImageTextRadioButton_TextMargin
      {
         get { return (Thickness)GetValue(SvgImageTextRadioButton_TextMarginProperty); }
         set { SetValue(SvgImageTextRadioButton_TextMarginProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_ImageSizeProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_ImageSize", typeof(int), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(20));

      public int SvgImageTextRadioButton_ImageSize
      {
         get { return (int)GetValue(SvgImageTextRadioButton_ImageSizeProperty); }
         set { SetValue(SvgImageTextRadioButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_IconGeometryProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_IconGeometry", typeof(Geometry), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(null));

      public Geometry SvgImageTextRadioButton_IconGeometry
      {
         get { return (Geometry)GetValue(SvgImageTextRadioButton_IconGeometryProperty); }
         set { SetValue(SvgImageTextRadioButton_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_CheckedIconGeometryProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_CheckedIconGeometry", typeof(Geometry), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(null));

      public Geometry SvgImageTextRadioButton_CheckedIconGeometry
      {
         get
         {
            return (Geometry)GetValue(SvgImageTextRadioButton_CheckedIconGeometryProperty);
         }
         set { SetValue(SvgImageTextRadioButton_CheckedIconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_CheckedBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextRadioButton_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextRadioButton_CheckedBackgroundColorProperty); }
         set { SetValue(SvgImageTextRadioButton_CheckedBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_OverBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextRadioButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextRadioButton_OverBackgroundColorProperty); }
         set { SetValue(SvgImageTextRadioButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextRadioButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextRadioButton_MouseDownBackgroundColorProperty); }
         set { SetValue(SvgImageTextRadioButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_ImageColorProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_ImageColor", typeof(SolidColorBrush), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgImageTextRadioButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextRadioButton_ImageColorProperty); }
         set { SetValue(SvgImageTextRadioButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextRadioButton_OrientationProperty =
        DependencyProperty.Register("SvgImageTextRadioButton_Orientation", typeof(OrientationType), typeof(SvgImageTextRadioButton), new UIPropertyMetadata(OrientationType.Horizontal));

      public OrientationType SvgImageTextRadioButton_Orientation
      {
         get { return (OrientationType)GetValue(SvgImageTextRadioButton_OrientationProperty); }
         set { SetValue(SvgImageTextRadioButton_OrientationProperty, value); }
      }

      static SvgImageTextRadioButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgImageTextRadioButton), new FrameworkPropertyMetadata(typeof(SvgImageTextRadioButton)));
      }
   }
}
