using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OtherSideCore.Wpf.CustomControls.Converters
{
   public class ZeroToVisibilityConverter : IValueConverter
   {
      public ZeroToVisibilityConverter() { }

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value is int)
         {
            int iValue = (int)value;
            if (iValue == 0)
               return parameter == null ? Visibility.Collapsed : Visibility.Visible;
            else
               return parameter == null ? Visibility.Visible : Visibility.Collapsed;
         }
         else if (value is double)
         {
            double dValue = (double)value;
            if (dValue == 0)
               return parameter == null ? Visibility.Collapsed : Visibility.Visible;
            else
               return parameter == null ? Visibility.Visible : Visibility.Collapsed;
         }
         else
         {
            throw new ArgumentException("Expected input is int or double");
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
