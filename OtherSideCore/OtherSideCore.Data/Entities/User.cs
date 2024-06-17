using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data
{
   public abstract class User : EntityBase
   {
      public int Id { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }

      public override List<DatabaseField> GetDatabaseFieldProperties()
      {
         var databaseFieldProperties = new List<DatabaseField>();

         databaseFieldProperties.Add(new IntegerDatabaseField(Id, "Id"));
         databaseFieldProperties.Add(new StringDatabaseField(FirstName, "FirstName"));
         databaseFieldProperties.Add(new StringDatabaseField(LastName, "LastName"));
         databaseFieldProperties.Add(new StringDatabaseField(UserName, "UserName"));

         return databaseFieldProperties;
      }
   }
}
