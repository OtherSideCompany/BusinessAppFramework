using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class ClickSelectTextBox : TextBox
   {
      public static readonly DependencyProperty ClickSelectTextBox_WatermarkProperty =
        DependencyProperty.Register("ClickSelectTextBox_Watermark", typeof(string), typeof(ClickSelectTextBox), new UIPropertyMetadata(String.Empty));

      public string ClickSelectTextBox_Watermark
      {
         get { return (string)GetValue(ClickSelectTextBox_WatermarkProperty); }
         set { SetValue(ClickSelectTextBox_WatermarkProperty, value); }
      }

      public static readonly DependencyProperty ClickSelectTextBox_TextMarginProperty =
        DependencyProperty.Register("ClickSelectTextBox_TextMargin", typeof(Thickness), typeof(ClickSelectTextBox), new UIPropertyMetadata(new Thickness(8,0,8,0)));

      public Thickness ClickSelectTextBox_TextMargin
      {
         get { return (Thickness)GetValue(ClickSelectTextBox_TextMarginProperty); }
         set { SetValue(ClickSelectTextBox_TextMarginProperty, value); }
      }

      public static readonly DependencyProperty ClickSelectTextBox_UnitsProperty =
        DependencyProperty.Register("ClickSelectTextBox_Units", typeof(string), typeof(ClickSelectTextBox), new UIPropertyMetadata(String.Empty));

      public string ClickSelectTextBox_Units
      {
         get { return (string)GetValue(ClickSelectTextBox_UnitsProperty); }
         set { SetValue(ClickSelectTextBox_UnitsProperty, value); }
      }

      public ClickSelectTextBox()
      {
         AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
         AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
         AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
      }

      private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
      {
         DependencyObject parent = e.OriginalSource as UIElement;

         while (parent != null && !(parent is TextBox))
         {
            parent = VisualTreeHelper.GetParent(parent);
         }

         if (parent != null)
         {
            var textBox = (TextBox)parent;

            if (!textBox.IsKeyboardFocusWithin)
            {
               textBox.Focus();
               e.Handled = true;
            }
         }
      }

      protected static void SelectAllText(object sender, RoutedEventArgs e)
      {
         var textBox = e.OriginalSource as TextBox;

         if (textBox != null)
         {
            textBox.SelectAll();
         }
      }
   }
}
