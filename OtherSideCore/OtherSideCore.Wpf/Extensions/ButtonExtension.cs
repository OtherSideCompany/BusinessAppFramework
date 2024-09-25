using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Extensions
{
   public class ButtonExtension
   {
      public static readonly DependencyProperty OverColorProperty =
        DependencyProperty.RegisterAttached("OverColor", typeof(Brush), typeof(ButtonExtension), new PropertyMetadata(Brushes.Transparent));

      public static void SetOverColor(UIElement element, Brush value)
      {
         element.SetValue(OverColorProperty, value);
      }

      public static Brush GetOverColor(UIElement element)
      {
         return (Brush)element.GetValue(OverColorProperty);
      }

      public static readonly DependencyProperty PressedColorProperty =
        DependencyProperty.RegisterAttached("PressedColor", typeof(Brush), typeof(ButtonExtension), new PropertyMetadata(Brushes.Transparent));

      public static void SetPressedColor(UIElement element, Brush value)
      {
         element.SetValue(PressedColorProperty, value);
      }

      public static Brush GetPressedColor(UIElement element)
      {
         return (Brush)element.GetValue(PressedColorProperty);
      }
   }
}
