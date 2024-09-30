using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class DecimalDatabaseField : DatabaseField
   {
      #region Fields

      private decimal _value;
      private string _buffer;

      #endregion

      #region Properties

      public decimal Value
      {
         get => _value;
         set
         {
            var updateDirtySate = !m_IsLoading && value != _value;

            if (IsEditable)
            {
               SetProperty(ref _value, value);

               if (updateDirtySate) IsDirty = true;
            }
         }
      }

      public string Buffer
      {
         get => _buffer;
         set
         {
            var trimmedValue = value.Trim();
            string pattern = @"^[+-]?\d*[\.,]?\d*$";
            var setProperty = Regex.IsMatch(trimmedValue, pattern);
            setProperty &= trimmedValue.Count(c => c == '.') <= 1;
            setProperty &= trimmedValue.Count(c => c == ',') <= 1;

            if (setProperty)
            {
               SetProperty(ref _buffer, trimmedValue);

               var valueToParse = _buffer;
               var isNegative = false;

               if (valueToParse.StartsWith("+") || valueToParse.StartsWith("-"))
               {
                  isNegative = valueToParse.StartsWith("-");
                  valueToParse = valueToParse.Substring(1);

                  if (valueToParse.Length == 0)
                  {
                     valueToParse = "0";
                  }
               }

               if (valueToParse.StartsWith(".") || valueToParse.StartsWith(","))
               {
                  valueToParse = "0" + valueToParse;
               }

               if (valueToParse.EndsWith(".") || valueToParse.EndsWith(","))
               {
                  valueToParse = valueToParse + "0";
               }

               valueToParse = valueToParse.Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
               valueToParse = valueToParse.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

               Value = decimal.Parse((isNegative ? "-" : "+") + valueToParse, System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat);
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DecimalDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {

      }

      #endregion

      #region Public Methods

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (decimal)value;
         LoadBuffer();

         m_IsLoading = false;
      }

      public void LoadBuffer()
      {
         Buffer = Value.ToString("0.00", System.Threading.Thread.CurrentThread.CurrentCulture);
      }

      #endregion
   }
}
