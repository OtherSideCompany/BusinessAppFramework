using System.Windows;

namespace OtherSideCore.Wpf.Extensions
{
   public class ComboBoxExtension
   {
      public static readonly DependencyProperty AdditionalContentProperty =
        DependencyProperty.RegisterAttached("AdditionalContent", typeof(FrameworkElement), typeof(ComboBoxExtension), new PropertyMetadata(null));

      public static void SetAdditionalContent(UIElement element, FrameworkElement value)
      {
         element.SetValue(AdditionalContentProperty, value);
      }

      public static FrameworkElement GetAdditionalContent(UIElement element)
      {
         return (FrameworkElement)element.GetValue(AdditionalContentProperty);
      }
   }
}
