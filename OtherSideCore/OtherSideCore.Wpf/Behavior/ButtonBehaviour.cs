using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Behavior
{
   public class ButtonBehaviour
   {
      public static readonly DependencyProperty CloseParentPopupOnClickProperty =
        DependencyProperty.RegisterAttached(
            "CloseParentPopupOnClick",
            typeof(bool),
            typeof(ButtonBehaviour),
            new PropertyMetadata(false, OnCloseParentPopupOnClickChanged));

      public static bool GetCloseParentPopupOnClick(Button button) => (bool)button.GetValue(CloseParentPopupOnClickProperty);

      public static void SetCloseParentPopupOnClick(Button button, bool value) => button.SetValue(CloseParentPopupOnClickProperty, value);

      private static void OnCloseParentPopupOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         if (d is Button button && e.NewValue is bool closeOnClick && closeOnClick)
         {
            button.Click += (sender, args) =>
            {
               var popup = FindParentPopup(button);

               if (popup != null)
               {
                  popup.IsOpen = false;
               }
            };
         }
      }

      private static Popup FindParentPopup(Button button)
      {
         DependencyObject parent = button;

         while (parent != null)
         {
            if (parent is Popup popup)
            {
               return popup;
            }
            parent = VisualTreeHelper.GetParent(parent);
         }

         return null;
      }
   }
}
