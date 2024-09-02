using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.DatabaseFields
{
   public class DateTimeDatabaseField : DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public DateTime Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DateTimeDatabaseField(DateTime value, string databaseFieldName) : base(databaseFieldName)
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
