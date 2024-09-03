using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgImageTextButton : Button
   {
      public static readonly DependencyProperty SvgImageTextButton_TextProperty =
         DependencyProperty.Register("SvgImageTextButton_Text", typeof(string), typeof(SvgImageTextButton), new UIPropertyMetadata(""));

      public string SvgImageTextButton_Text
      {
         get { return (string)GetValue(SvgImageTextButton_TextProperty); }
         set { SetValue(SvgImageTextButton_TextProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_TextMarginProperty =
        DependencyProperty.Register("SvgImageTextButton_TextMargin", typeof(Thickness), typeof(SvgImageTextButton), new UIPropertyMetadata(new Thickness(0, 0, 5, 0)));

      public Thickness SvgImageTextButton_TextMargin
      {
         get { return (Thickness)GetValue(SvgImageTextButton_TextMarginProperty); }
         set { SetValue(SvgImageTextButton_TextMarginProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_ImageSizeProperty =
        DependencyProperty.Register("SvgImageTextButton_ImageSize", typeof(int), typeof(SvgImageTextButton), new UIPropertyMetadata((int)24));

      public int SvgImageTextButton_ImageSize
      {
         get { return (int)GetValue(SvgImageTextButton_ImageSizeProperty); }
         set { SetValue(SvgImageTextButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_IconGeometryProperty =
        DependencyProperty.Register("SvgImageTextButton_IconGeometry", typeof(Geometry), typeof(SvgImageTextButton), new UIPropertyMetadata(null));

      public Geometry SvgImageTextButton_IconGeometry
      {
         get { return (Geometry)GetValue(SvgImageTextButton_IconGeometryProperty); }
         set { SetValue(SvgImageTextButton_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_OverBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextButton_OverBackgroundColorProperty); }
         set { SetValue(SvgImageTextButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("SvgImageTextButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(SvgImageTextButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush SvgImageTextButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextButton_MouseDownBackgroundColorProperty); }
         set { SetValue(SvgImageTextButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty SvgImageTextButton_ImageColorProperty =
        DependencyProperty.Register("SvgImageTextButton_ImageColor", typeof(SolidColorBrush), typeof(SvgImageTextButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgImageTextButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgImageTextButton_ImageColorProperty); }
         set { SetValue(SvgImageTextButton_ImageColorProperty, value); }
      }

      static SvgImageTextButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgImageTextButton), new FrameworkPropertyMetadata(typeof(SvgImageTextButton)));
      }
   }
}
