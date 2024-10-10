using System;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class NullToVisibilityConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value == null)
         {
            return parameter == null ? Visibility.Collapsed : Visibility.Visible;
         }

         return parameter == null ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
