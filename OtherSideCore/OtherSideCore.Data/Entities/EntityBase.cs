using OtherSideCore.Data.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Entities
{
   public class EntityBase
   {
      protected void SetProperties(List<DatabaseField> databaseFields)
      {
         foreach (var databaseField in databaseFields)
         {
            PropertyInfo propertyInfo = GetType().GetProperty(databaseField.DatabaseFieldName);
            propertyInfo.SetValue(this, databaseField.Value, null);
         }
      }
   }
}
