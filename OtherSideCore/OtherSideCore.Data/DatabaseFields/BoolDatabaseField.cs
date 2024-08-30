using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.DatabaseFields
{
   public class BoolDatabaseField : DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public bool Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public BoolDatabaseField(bool value, string databaseFieldName) : base(databaseFieldName)
      {
         Value = value;
      }

      #endregion

      #region Methods

      public override string GetFormattedValue()
      {
         return Value.ToString();
      }

      #endregion
   }
}
