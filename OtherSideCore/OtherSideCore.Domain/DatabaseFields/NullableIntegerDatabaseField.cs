using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class NullableIntegerDatabaseField : DatabaseField
   {
      #region Fields

      private int? m_Value;
      private string _buffer;

      #endregion

      #region Properties

      public int? Value
      {
         get => m_Value;
         set
         {
            var updateDirtySate = !m_IsLoading && value != m_Value;

            if (IsEditable)
            {
               SetProperty(ref m_Value, value);

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

            if (string.IsNullOrEmpty(trimmedValue))
            {
               SetProperty(ref _buffer, trimmedValue);
               Value = null;
            }
            else
            {               
               string pattern = @"^[+-]?(\d+)?$";
               var setProperty = Regex.IsMatch(trimmedValue, pattern);

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

                  Value = int.Parse((isNegative ? "-" : "+") + valueToParse, System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat);
               }
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public NullableIntegerDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {

      }

      #endregion

      #region Public Methods

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (int?)value;
         LoadBuffer();

         m_IsLoading = false;
      }

      public void LoadBuffer()
      {
         Buffer = Value.HasValue ? Value.Value.ToString(System.Threading.Thread.CurrentThread.CurrentCulture) : "";
      }

      #endregion
   }
}
