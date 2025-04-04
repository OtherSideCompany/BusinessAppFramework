using System;
using System.Globalization;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class IntEqualsConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null || parameter == null)
         {
            return false;
         }

         return value.Equals(int.Parse(parameter.ToString()));
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if ((bool)value && parameter != null)
         {
            return int.Parse(parameter.ToString());
         }

         return Binding.DoNothing;
      }
   }
}
