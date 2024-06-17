using OtherSideCore.Data.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Entities
{
   public abstract class EntityBase
   {
      public abstract List<DatabaseField> GetDatabaseFieldProperties();

      protected void SetProperties(List<DatabaseField> databaseFields)
      {
         foreach (var databaseField in databaseFields)
         {
            PropertyInfo propertyInfo = GetType().GetProperty(databaseField.DatabaseFieldName);

            switch (databaseField)
            {
               case IntegerDatabaseField integerDatabaseField:
                  propertyInfo.SetValue(this, integerDatabaseField.Value, null);
                  break;
               case StringDatabaseField stringDatabaseField:
                  propertyInfo.SetValue(this, stringDatabaseField.Value, null);
                  break;
               default:
                  throw new Exception("Unrecognized type " + databaseField.GetType());
                  break;
            }
         }
      }

      public abstract int Create(List<DatabaseField> databaseFields);

      public abstract void Save(int id, List<DatabaseField> databaseFields);

      public abstract EntityBase Get(int id);

      public abstract void Delete(int id);
      
   }
}
