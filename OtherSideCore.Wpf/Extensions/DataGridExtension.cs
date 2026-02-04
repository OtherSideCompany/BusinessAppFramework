using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Extensions
{
   public class DataGridExtension
   {
      public static readonly DependencyProperty ThumbColorProperty =
        DependencyProperty.RegisterAttached("ThumbColor", typeof(Brush), typeof(DataGridExtension), new PropertyMetadata(Brushes.Black));

      public static void SetThumbColor(UIElement element, Brush value)
      {
         element.SetValue(ThumbColorProperty, value);
      }

      public static Brush GetThumbColor(UIElement element)
      {
         return (Brush)element.GetValue(ThumbColorProperty);
      }     

      public static readonly DependencyProperty HeaderForegroundColorProperty =
        DependencyProperty.RegisterAttached("HeaderForegroundColor", typeof(Brush), typeof(DataGridExtension), new PropertyMetadata(Brushes.LightBlue));

      public static void SetHeaderForegroundColor(UIElement element, Brush value)
      {
         element.SetValue(HeaderForegroundColorProperty, value);
      }

      public static Brush GetHeaderForegroundColor(UIElement element)
      {
         return (Brush)element.GetValue(HeaderForegroundColorProperty);
      }     
   }
}
