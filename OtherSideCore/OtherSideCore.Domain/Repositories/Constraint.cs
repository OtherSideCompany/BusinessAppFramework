using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
   public class Constraint
   {
      #region Fields



      #endregion

      #region Properties

      public string Name { get; private set; }
      public string PropertyName { get; private set; }
      public object Value { get; private set; }

      public static Constraint Empty => new Constraint("Tout", "", null);

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Constraint(string name, string propertyName, object value)
      {
         Name = name;
         PropertyName = propertyName;
         Value = value;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
