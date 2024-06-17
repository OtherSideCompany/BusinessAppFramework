using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.DatabaseFields
{
   public class IntegerDatabaseField : DatabaseField
   {
      #region Fields

      private int m_Value;

      #endregion

      #region Properties

      public int Value
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
               OnPropertyChanged("Value");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public IntegerDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {

      }

      public override void LoadValue(object value)
      {
         Value = (int)value;
      }

      #endregion

      #region Methods



      #endregion
   }
}
