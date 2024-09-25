using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.DatabaseFields
{
   public class DecimalDatabaseField : DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public decimal Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DecimalDatabaseField(decimal value, string databaseFieldName) : base(databaseFieldName)
      {
         Value = value;
      }


      #endregion

      #region Public Methods

      public override string GetFormattedValue()
      {
         return Value.ToString();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
