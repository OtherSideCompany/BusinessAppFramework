using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class ImageToggleButton : ToggleButton
   {
      public static readonly DependencyProperty ImageToggleButton_ImageSizeProperty =
        DependencyProperty.Register("ImageToggleButton_ImageSize", typeof(int), typeof(ImageToggleButton), new UIPropertyMetadata((int)24));

      public int ImageToggleButton_ImageSize
      {
         get { return (int)GetValue(ImageToggleButton_ImageSizeProperty); }
         set { SetValue(ImageToggleButton_ImageSizeProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_BackgroundImageProperty =
        DependencyProperty.Register("ImageToggleButton_BackgroundImage", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

      public ImageSource ImageToggleButton_BackgroundImage
      {
         get { return (ImageSource)GetValue(ImageToggleButton_BackgroundImageProperty); }
         set { SetValue(ImageToggleButton_BackgroundImageProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_OverImageProperty =
        DependencyProperty.Register("ImageToggleButton_OverImage", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

      public ImageSource ImageToggleButton_OverImage
      {
         get
         { return (ImageSource)GetValue(ImageToggleButton_OverImageProperty); }
         set { SetValue(ImageToggleButton_OverImageProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_CheckedImageProperty =
        DependencyProperty.Register("ImageToggleButton_CheckedImage", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

      public ImageSource ImageToggleButton_CheckedImage
      {
         get { return (ImageSource)GetValue(ImageToggleButton_CheckedImageProperty); }
         set { SetValue(ImageToggleButton_CheckedImageProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_CheckedBackgroundColorProperty =
        DependencyProperty.Register("ImageToggleButton_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(ImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush ImageToggleButton_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(ImageToggleButton_CheckedBackgroundColorProperty); }
         set { SetValue(ImageToggleButton_CheckedBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_OverBackgroundColorProperty =
        DependencyProperty.Register("ImageToggleButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(ImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush ImageToggleButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(ImageToggleButton_OverBackgroundColorProperty); }
         set { SetValue(ImageToggleButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty ImageToggleButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("ImageToggleButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(ImageToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush ImageToggleButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(ImageToggleButton_MouseDownBackgroundColorProperty); }
         set { SetValue(ImageToggleButton_MouseDownBackgroundColorProperty, value); }
      }
   }
}
