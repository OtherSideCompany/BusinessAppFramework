using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OtherSideCore.Wpf.Converters
{
   public class DecimalInputConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is decimal decimalValue)
         {
            return decimalValue.ToString(culture); // Format decimal according to culture
         }
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         //var input = value as string;
         //if (!string.IsNullOrWhiteSpace(input))
         //{
         //   input = input.Replace(",", culture.NumberFormat.CurrencyDecimalSeparator)
         //                .Replace(".", culture.NumberFormat.CurrencyDecimalSeparator);

         //   if (decimal.TryParse(input, NumberStyles.Number, culture, out decimal result))
         //   {
         //      return result;
         //   }
         //}

         //return 0m; // Default value in case of invalid input

         var input = value as string;

         string pattern = @"^[+-]?\d*[\.,]?\d*$";
         var matchPattern = Regex.IsMatch(input, pattern);
         
         if (matchPattern)
         {
            return input;
         }
         else
         {
            return input.Substring(0, input.Length - 1);
         }
      }

      //private decimal test(string value)
      //{
      //   string pattern = @"^[+-]?\d*[\.,]?\d*$";
      //   var setProperty = Regex.IsMatch(value, pattern);
      //   setProperty &= value.Count(c => c == '.') <= 1;
      //   setProperty &= value.Count(c => c == ',') <= 1;

      //   if (setProperty)
      //   {
      //      //SetProperty(ref _buffer, value);

      //      var valueToParse = value;
      //      var isNegative = false;

      //      if (valueToParse.StartsWith("+") || valueToParse.StartsWith("-"))
      //      {
      //         isNegative = valueToParse.StartsWith("-");
      //         valueToParse = valueToParse.Substring(1);

      //         if (valueToParse.Length == 0)
      //         {
      //            valueToParse = "0";
      //         }
      //      }

      //      if (valueToParse.StartsWith(".") || valueToParse.StartsWith(","))
      //      {
      //         valueToParse = "0" + valueToParse;
      //      }

      //      if (valueToParse.EndsWith(".") || valueToParse.EndsWith(","))
      //      {
      //         valueToParse = valueToParse + "0";
      //      }

      //      valueToParse = valueToParse.Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      //      valueToParse = valueToParse.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

      //      return decimal.Parse((isNegative ? "-" : "+") + valueToParse, System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat);
      //   }
      //}
   }
}
