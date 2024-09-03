using OtherSideCore.Infrastructure.DatabaseFields;
using System.Collections.Generic;

namespace OtherSideCore.Infrastructure.Entities
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
