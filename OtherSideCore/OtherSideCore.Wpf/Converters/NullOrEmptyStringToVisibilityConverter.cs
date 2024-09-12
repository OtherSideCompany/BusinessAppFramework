using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class NullOrEmptyStringToVisibilityConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value is string)
         {
            if (parameter == null)
            {
               return String.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
               return String.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
            }
         }

         return Visibility.Collapsed;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
