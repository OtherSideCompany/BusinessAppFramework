using System;
using System.Globalization;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class MonthNumberToNameConverter : IValueConverter
   {
      public bool UseAbbreviatedName { get; set; } = false;

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is int month && month >= 1 && month <= 12)
         {
            var dateTimeFormat = culture.DateTimeFormat;
            var monthName = parameter != null  ? dateTimeFormat.GetAbbreviatedMonthName(month) : dateTimeFormat.GetMonthName(month);
            return char.ToUpper(monthName[0]) + monthName.Substring(1);
         }
         else
         {
            return '-';
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotSupportedException("MonthNumberToNameConverter is one-way only.");
      }
   }
}
