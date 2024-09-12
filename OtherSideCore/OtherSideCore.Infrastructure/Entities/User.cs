using OtherSideCore.Infrastructure.DatabaseFields;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtherSideCore.Infrastructure.Entities
{
   [Table("Users")]
   public class User : EntityBase
   {
      public bool IsActive { get; set; }

      [Required]
      [StringLength(50)]
      public string FirstName { get; set; }

      [Required]
      [StringLength(50)]
      public string LastName { get; set; }

      [Required]
      [StringLength(50)]
      public string UserName { get; set; }

      [Required]
      [StringLength(64, MinimumLength = 64)]
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
