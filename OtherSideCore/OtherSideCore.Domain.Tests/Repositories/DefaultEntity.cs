using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using System.Collections.Generic;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class DefaultEntity : EntityBase
   {
      public string RandomProperty { get; set; }

      public override List<DatabaseField> GetDatabaseFieldProperties()
      {
         var databaseFieldProperties = base.GetDatabaseFieldProperties();

         databaseFieldProperties.Add(new StringDatabaseField(RandomProperty, nameof(RandomProperty)));

         return databaseFieldProperties;
      }
   }
}
