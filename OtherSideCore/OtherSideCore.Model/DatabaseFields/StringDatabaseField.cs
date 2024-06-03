using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.DatabaseFields
{
   public class StringDatabaseField : DatabaseField
   {
      #region Fields

      private string m_Value;
      private string m_Buffer;
      private int m_MaxLength;

      #endregion

      #region Properties

      public string Value
      {
         get
         {
            return m_Value;
         }
         set
         {
            if (value != m_Value)
            {
               m_Value = value;
               OnPropertyChanged(nameof(Value));
            }
         }
      }

      public string Buffer
      {
         get
         {
            return m_Buffer;
         }
         set
         {
            if (value != m_Buffer)
            {
               m_Buffer = value;
               OnPropertyChanged(nameof(Buffer));
            }
         }
      }

      public int MaxLength
      {
         get
         {
            return m_MaxLength;
         }
         set
         {
            if (value != m_MaxLength)
            {
               m_MaxLength = value;
               OnPropertyChanged(nameof(MaxLength));
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public StringDatabaseField(string databaseFieldName, int maxLength) : base(databaseFieldName)
      {
         MaxLength = maxLength;
         Value = GlobalVariables.DefaultString;
      }

      #endregion

      #region Methods

      public void LoadBuffer()
      {

      }

      public void UpdateBuffer()
      {

      }

      public override bool IsValid()
      {
         return Value.Length < MaxLength;
      }

      #endregion
   }
}
