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
   public class ImageButton : Button
   {
      public static readonly DependencyProperty ImageButton_ImageSizeProperty =
        DependencyProperty.Register("ImageButton_ImageSize", typeof(int), typeof(ImageButton), new UIPropertyMetadata((int)24));

      public int ImageButton_ImageSize
      {
         get { return (int)GetValue(ImageButton_ImageSizeProperty); }
         set { SetValue(ImageButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty ImageButton_BackgroundImageProperty =
        DependencyProperty.Register("ImageButton_BackgroundImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

      public ImageSource ImageButton_BackgroundImage
      {
         get { return (ImageSource)GetValue(ImageButton_BackgroundImageProperty); }
         set { SetValue(ImageButton_BackgroundImageProperty, value); }
      }

      public static readonly DependencyProperty ImageButton_OverBackgroundColorProperty =
        DependencyProperty.Register("ImageButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(ImageButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush ImageButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(ImageButton_OverBackgroundColorProperty); }
         set { SetValue(ImageButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty ImageButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("ImageButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(ImageButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush ImageButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(ImageButton_MouseDownBackgroundColorProperty); }
         set { SetValue(ImageButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty ImageButton_OverImageProperty =
        DependencyProperty.Register("ImageButton_OverImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

      public ImageSource ImageButton_OverImage
      {
         get
         {
            var value = (ImageSource)GetValue(ImageButton_OverImageProperty);
            return value != null ? value : (ImageSource)GetValue(ImageButton_BackgroundImageProperty);
         }
         set { SetValue(ImageButton_OverImageProperty, value); }
      }

      public static readonly DependencyProperty ImageButton_MouseDownImageProperty =
        DependencyProperty.Register("ImageButton_MouseDownImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

      public ImageSource ImageButton_MouseDownImage
      {
         get
         {
            var value = (ImageSource)GetValue(ImageButton_MouseDownImageProperty);
            return value != null ? value : (ImageSource)GetValue(ImageButton_BackgroundImageProperty);
         }
         set { SetValue(ImageButton_MouseDownImageProperty, value); }
      }

      static ImageButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
      }

      public ImageButton()
      {
         SetResourceReference(StyleProperty, typeof(ImageButton));
      }
   }
}
