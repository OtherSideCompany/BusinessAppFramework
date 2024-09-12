using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class BoolToVisibleOrHidden : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         bool bValue = (bool)value;
         if (bValue)
            return parameter == null ? Visibility.Visible : Visibility.Hidden;
         else
            return parameter == null ? Visibility.Hidden : Visibility.Visible;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
