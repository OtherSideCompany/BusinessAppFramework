using System.Windows.Controls;
using System.Windows;

namespace OtherSideCore.Wpf.Behavior
{
   public class TextBoxBehaviour
   {
      #region IsDecimal Proprety

      public static readonly DependencyProperty IsDecimalProperty = DependencyProperty.RegisterAttached(
            "IsDecimal",
            typeof(bool),
            typeof(TextBoxBehaviour),
            new FrameworkPropertyMetadata(false, OnIsDecimalChanged));

      public static bool GetIsDecimal(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsDecimalProperty);
      }

      public static void SetIsDecimal(DependencyObject obj, bool value)
      {
         obj.SetValue(IsDecimalProperty, value);
      }

      private static RoutedEventHandler OnDecimalLostFocusHandler = new RoutedEventHandler(OnDecimalTextBox_LostFocus);

      private static void OnIsDecimalChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
      {
         Control textBox = owner as TextBox;
         bool? isDecimal = args.NewValue as bool?;

         if (isDecimal == null || textBox == null)
            return;

         if (isDecimal ?? false)
         {
            textBox.LostFocus += OnDecimalLostFocusHandler;
         }
         else
         {
            textBox.LostFocus -= OnDecimalLostFocusHandler;
         }
      }

      private static void OnDecimalTextBox_LostFocus(object sender, RoutedEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = Application.Utils.RemoveInvalidCharacterForDecimal(textBox.Text);
      }

      #endregion

      #region IsInteger Proprety

      public static readonly DependencyProperty IsIntegerProperty = DependencyProperty.RegisterAttached(
            "IsInteger",
            typeof(bool),
            typeof(TextBoxBehaviour),
            new FrameworkPropertyMetadata(false, OnIsIntegerChanged));

      public static bool GetIsInteger(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsIntegerProperty);
      }

      public static void SetIsInteger(DependencyObject obj, bool value)
      {
         obj.SetValue(IsIntegerProperty, value);
      }

      private static RoutedEventHandler OnIntegerLostFocusHandler = new RoutedEventHandler(OnIntegerTextBox_LostFocus);

      private static void OnIsIntegerChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
      {
         Control textBox = owner as TextBox;
         bool? isInteger = args.NewValue as bool?;

         if (isInteger == null || textBox == null)
            return;

         if (isInteger ?? false)
         {
            textBox.LostFocus += OnIntegerLostFocusHandler;
         }
         else
         {
            textBox.LostFocus -= OnIntegerLostFocusHandler;
         }
      }

      private static void OnIntegerTextBox_LostFocus(object sender, RoutedEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = Application.Utils.RemoveInvalidCharacterForInteger(textBox.Text);
      }

      #endregion

      #region IsNullableInteger Proprety

      public static readonly DependencyProperty IsNullableIntegerProperty = DependencyProperty.RegisterAttached(
            "IsNullableInteger",
            typeof(bool),
            typeof(TextBoxBehaviour),
            new FrameworkPropertyMetadata(false, OnIsNullableIntegerChanged));

      public static bool GetIsNullableInteger(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsNullableIntegerProperty);
      }

      public static void SetIsNullableInteger(DependencyObject obj, bool value)
      {
         obj.SetValue(IsNullableIntegerProperty, value);
      }

      private static RoutedEventHandler OnNullableIntegerLostFocusHandler = new RoutedEventHandler(OnNullableIntegerTextBox_LostFocus);

      private static void OnIsNullableIntegerChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
      {
         Control textBox = owner as TextBox;
         bool? isInteger = args.NewValue as bool?;

         if (isInteger == null || textBox == null)
            return;

         if (isInteger ?? false)
         {
            textBox.LostFocus += OnNullableIntegerLostFocusHandler;
         }
         else
         {
            textBox.LostFocus -= OnNullableIntegerLostFocusHandler;
         }
      }

      private static void OnNullableIntegerTextBox_LostFocus(object sender, RoutedEventArgs e)
      {
         var textBox = sender as TextBox;

         string input = textBox.Text?.Trim();

         if (string.IsNullOrEmpty(input))
         {
            return;
         }

         textBox.Text = Application.Utils.RemoveInvalidCharacterForInteger(textBox.Text);
      }

      #endregion
   }
}
