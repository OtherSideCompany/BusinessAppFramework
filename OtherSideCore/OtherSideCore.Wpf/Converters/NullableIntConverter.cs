using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class NullableIntConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is int intValue)
            return intValue.ToString(culture);

         return string.Empty;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var str = value as string;

         if (string.IsNullOrWhiteSpace(str))
            return null;

         if (int.TryParse(str, NumberStyles.Integer, culture, out var result))
            return result;

         return DependencyProperty.UnsetValue;
      }
   }
}
