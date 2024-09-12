using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OtherSideCore.Infrastructure.DatabaseFields;

namespace OtherSideCore.Infrastructure.Entities
{
   public abstract class EntityBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public DateTime CreationDate { get; set; }
      public int? CreatedById { get; set; }
      public virtual User CreatedBy { get; set; }
      public DateTime LastModifiedDateTime { get; set; }
      public int? LastModifiedById { get; set; }
      public virtual User LastModifiedBy { get; set; }

      internal void SetProperties(List<DatabaseField> databaseFields)
      {
         foreach (var databaseField in databaseFields)
         {
            PropertyInfo propertyInfo = GetType().GetProperty(databaseField.DatabaseFieldName);

            if (propertyInfo != null)
            {
               switch (databaseField)
               {
                  case IntegerDatabaseField integerDatabaseField:
                     propertyInfo.SetValue(this, integerDatabaseField.Value);
                     break;
                  case StringDatabaseField stringDatabaseField:
                     propertyInfo.SetValue(this, stringDatabaseField.Value);
                     break;
                  case DateOnlyDatabaseField dateOnlyDatabaseField:
                     propertyInfo.SetValue(this, dateOnlyDatabaseField.Value);
                     break;
                  case DateTimeDatabaseField dateTimeDatabaseField:
                     propertyInfo.SetValue(this, dateTimeDatabaseField.Value);
                     break;
                  case BoolDatabaseField boolDatabaseField:
                     propertyInfo.SetValue(this, boolDatabaseField.Value);
                     break;
                  case NullableIntegerDatabaseField nullableIntegerDatabaseField:
                     propertyInfo.SetValue(this, nullableIntegerDatabaseField.Value);
                     break;
                  default:
                     throw new ArgumentException("Unrecognized DatabaseField type " + databaseField.GetType());
               }
            }
            else
            {
               throw new ArgumentException("Cannot find property '" + databaseField.DatabaseFieldName + "' in entity " + GetType().Name);
            }
         }
      }

      public virtual List<DatabaseField> GetDatabaseFieldProperties()
      {
         return new List<DatabaseField>
            {
               new IntegerDatabaseField(Id, nameof(Id)),
               new DateTimeDatabaseField(CreationDate, nameof(CreationDate)),
               new NullableIntegerDatabaseField(CreatedById, nameof(CreatedById)),
               new DateTimeDatabaseField(LastModifiedDateTime, nameof(LastModifiedDateTime)),
               new NullableIntegerDatabaseField(LastModifiedById, nameof(LastModifiedById))
            };
      }

   }
}
