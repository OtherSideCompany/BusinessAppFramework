using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class KeyToValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is object key && parameter is Dictionary<object, string> dictionary)
         {
            if (dictionary.TryGetValue(key, out var result))
            {
               return result;
            }
         }

         return "Unknown";
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotSupportedException();
      }
   }
}
