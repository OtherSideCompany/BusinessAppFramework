using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.DatabaseFields
{
   public class NullableIntegerDatabaseField : DatabaseField
   {

      #region Fields



      #endregion

      #region Properties

      public int? Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public NullableIntegerDatabaseField(int? value, string databaseFieldName) : base(databaseFieldName)
      {
         Value = value;
      }

      #endregion

      #region Public Methods

      public override string GetFormattedValue()
      {
         return Value.HasValue ? Value.Value.ToString() : "";
      }

      #endregion
   }
}
