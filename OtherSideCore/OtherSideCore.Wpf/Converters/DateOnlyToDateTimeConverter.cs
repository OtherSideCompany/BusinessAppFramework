using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class DateOnlyToDateTimeConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value is DateOnly dateOnly)
         {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
         }
         return DependencyProperty.UnsetValue;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value is DateTime dateTime)
         {
            return DateOnly.FromDateTime(dateTime);
         }
         return DependencyProperty.UnsetValue;
      }
   }
}
