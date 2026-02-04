using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Extensions
{
   public class TreeViewExtension
   {
      public static readonly DependencyProperty TreeViewSelectedItemBackgroundColorProperty =
        DependencyProperty.RegisterAttached("TreeViewSelectedItemBackgroundColor", typeof(Brush), typeof(TreeViewExtension), new PropertyMetadata(Brushes.LightBlue));

      public static void SetTreeViewSelectedItemBackgroundColor(UIElement element, Brush value)
      {
         element.SetValue(TreeViewSelectedItemBackgroundColorProperty, value);
      }

      public static Brush GetTreeViewSelectedItemBackgroundColor(UIElement element)
      {
         return (Brush)element.GetValue(TreeViewSelectedItemBackgroundColorProperty);
      }
   }
}
