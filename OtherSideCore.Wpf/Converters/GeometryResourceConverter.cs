using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Converters
{
   public class GeometryResourceConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string resourceKey)
         {
            return System.Windows.Application.Current.TryFindResource(resourceKey) as Geometry;
         }

         return null;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
