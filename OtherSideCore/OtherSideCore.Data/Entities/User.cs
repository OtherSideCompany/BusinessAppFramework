using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Entities
{
   public class User : EntityBase
   {
      public bool IsSuperAdmin { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }

      public override List<DatabaseField> GetDatabaseFieldProperties()
      {
         var databaseFieldProperties = base.GetDatabaseFieldProperties();

         databaseFieldProperties.Add(new BoolDatabaseField(IsSuperAdmin, "IsSuperAdmin"));
         databaseFieldProperties.Add(new StringDatabaseField(FirstName, "FirstName"));
         databaseFieldProperties.Add(new StringDatabaseField(LastName, "LastName"));
         databaseFieldProperties.Add(new StringDatabaseField(UserName, "UserName"));

         return databaseFieldProperties;
      }
   }
}
