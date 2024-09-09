using OtherSideCore.Infrastructure.DatabaseFields;
using System.Collections.Generic;

namespace OtherSideCore.Infrastructure.Entities
{
   public class User : EntityBase
   {
      public bool IsActive { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }
      public string PasswordHash { get; set; }

      public override List<DatabaseField> GetDatabaseFieldProperties()
      {
         var databaseFieldProperties = base.GetDatabaseFieldProperties();

         databaseFieldProperties.Add(new BoolDatabaseField(IsActive, nameof(IsActive)));
         databaseFieldProperties.Add(new StringDatabaseField(FirstName, nameof(FirstName)));
         databaseFieldProperties.Add(new StringDatabaseField(LastName, nameof(LastName)));
         databaseFieldProperties.Add(new StringDatabaseField(UserName, nameof(UserName)));
         databaseFieldProperties.Add(new StringDatabaseField(PasswordHash, nameof(PasswordHash)));

         return databaseFieldProperties;
      }
   }
}
