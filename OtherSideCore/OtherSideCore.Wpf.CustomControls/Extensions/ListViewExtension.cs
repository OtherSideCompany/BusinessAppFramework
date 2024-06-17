using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace OtherSideCore.Wpf.CustomControls.Extensions
{
   public class ListViewExtension
   {
      public static readonly DependencyProperty ThumbColorProperty =
        DependencyProperty.RegisterAttached("ThumbColor", typeof(Brush), typeof(ListViewExtension), new PropertyMetadata(Brushes.Black));

      public static void SetThumbColor(UIElement element, Brush value)
      {
         element.SetValue(ThumbColorProperty, value);
      }

      public static Brush GetThumbColor(UIElement element)
      {
         return (Brush)element.GetValue(ThumbColorProperty);
      }

      public static readonly DependencyProperty SelectedItemBackgroundColorProperty =
        DependencyProperty.RegisterAttached("SelectedItemBackgroundColor", typeof(Brush), typeof(ListViewExtension), new PropertyMetadata(Brushes.LightBlue));

      public static void SetSelectedItemBackgroundColor(UIElement element, Brush value)
      {
         element.SetValue(SelectedItemBackgroundColorProperty, value);
      }

      public static Brush GetSelectedItemBackgroundColor(UIElement element)
      {
         return (Brush)element.GetValue(SelectedItemBackgroundColorProperty);
      }

      public static readonly DependencyProperty HeaderForegroundColorProperty =
        DependencyProperty.RegisterAttached("HeaderForegroundColor", typeof(Brush), typeof(ListViewExtension), new PropertyMetadata(Brushes.LightBlue));

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
