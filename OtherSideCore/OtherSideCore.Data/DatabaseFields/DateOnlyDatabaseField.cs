using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.DatabaseFields
{
   public class DateOnlyDatabaseField : DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public DateOnly Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DateOnlyDatabaseField(DateOnly value, string databaseFieldName) : base(databaseFieldName)
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
