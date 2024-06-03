using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.DatabaseFields
{
   public class StringDatabaseField : DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public string Value { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public StringDatabaseField(string value, string databaseFieldName) : base(databaseFieldName)
      {
         Value = value;
      }

      #endregion

      #region Methods



      #endregion
   }
}
