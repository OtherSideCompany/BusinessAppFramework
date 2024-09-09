using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.DatabaseFields
{
   public abstract class DatabaseField
   {
      #region Fields



      #endregion

      #region Properties

      public string DatabaseFieldName { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DatabaseField(string databaseFieldName)
      {
         DatabaseFieldName = databaseFieldName;
      }

      #endregion

      #region Public Methods

      public abstract string GetFormattedValue();

      #endregion
   }
}
