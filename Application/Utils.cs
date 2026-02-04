using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application
{
   public class Utils
   {
      public static int EditDistance(string s, string t, int maxSearchDistance)
      {
         if (s.Length == t.Length)
         {
            return LevenshteinDistance(s, t);
         }
         else if (s.Length > t.Length)
         {
            return LevenshteinDistance(s, t);
         }
         else
         {
            var minimalDistance = t.Length;

            for (int i = 0; i < t.Length - s.Length; i++)
            {
               var tSubString = t.Substring(i, s.Length);
               minimalDistance = Math.Min(LevenshteinDistance(s, tSubString), minimalDistance);
            }

            return Math.Min(minimalDistance, LevenshteinDistance(s, t));
         }
      }

      public static int GetMaxSearchDistance(string s)
      {
         return string.IsNullOrEmpty(s) ? 0 : Math.Max((int)Math.Round(s.Length / 2.0), 1);
      }

      public static int LevenshteinDistance(string s, string t)
      {
         if (s == t)
            return 0;
         if (string.IsNullOrEmpty(s))
            return string.IsNullOrEmpty(t) ? 0 : t.Length;
         if (string.IsNullOrEmpty(t))
            return string.IsNullOrEmpty(s) ? 0 : s.Length;

         int[,] d = new int[s.Length + 1, t.Length + 1];

         for (int i = 0; i <= s.Length; i++)
            d[i, 0] = i;
         for (int j = 0; j <= t.Length; j++)
            d[0, j] = j;

         for (int i = 1; i <= s.Length; i++)
         {
            for (int j = 1; j <= t.Length; j++)
            {
               int cost = t[j - 1] == s[i - 1] ? 0 : 1;
               d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
         }

         return d[s.Length, t.Length];
      }

      public static string RemoveInvalidCharacterForDecimal(string decimalString)
      {
         decimalString = Regex.Replace(decimalString, "[^0-9.,-]", "");

         bool encounteredDotOrComma = false;
         StringBuilder newText = new StringBuilder();

         foreach (char c in decimalString)
         {
            if (c == '.' || c == ',')
            {
               if (!encounteredDotOrComma)
               {
                  encounteredDotOrComma = true;
                  newText.Append(c);
               }
            }
            else
            {
               newText.Append(c);
            }
         }

         decimalString = newText.ToString();

         decimalString = decimalString.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
         decimalString = decimalString.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

         return decimalString;
      }

      public static string RemoveInvalidCharacterForInteger(string integerString)
      {
         integerString = Regex.Replace(integerString, "[^0-9+-]", "");

         if (integerString.Length > 1 && (integerString[0] == '+' || integerString[0] == '-'))
         {
            integerString = integerString[0] + integerString.Substring(1).Replace("+", "").Replace("-", "");
         }
         else
         {
            integerString = integerString.Replace("+", "").Replace("-", "");
         }

         return integerString;
      }
   }
}
