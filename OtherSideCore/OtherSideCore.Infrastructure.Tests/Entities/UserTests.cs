using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests.Entities
{
   public class UserTests
   {
      [Fact]
      public void SetProperties_ShouldSetPropertiesCorrectly()
      {
         var user = new User();

         Assert.False(user.IsSuperAdmin);
         Assert.NotEqual("SuperUserFirstName", user.FirstName);
         Assert.NotEqual("SuperUserLastName", user.LastName);
         Assert.NotEqual("SuperUserUserName", user.UserName);

         var databaseFields = new List<DatabaseField>
               {
                   new BoolDatabaseField(true, nameof(User.IsSuperAdmin)),
                   new StringDatabaseField("SuperUserFirstName", nameof(User.FirstName)),
                   new StringDatabaseField("SuperUserLastName", nameof(User.LastName)),
                   new StringDatabaseField("SuperUserUserName", nameof(User.UserName))
               };

         user.SetProperties(databaseFields);

         Assert.True(user.IsSuperAdmin);
         Assert.Equal("SuperUserFirstName", user.FirstName);
         Assert.Equal("SuperUserLastName", user.LastName);
         Assert.Equal("SuperUserUserName", user.UserName);
      }

      [Fact]
      public void GetDatabaseFieldProperties_ReturnsCorrectDatabaseFields()
      {
         var user = new User();

         var propertyCount = user.GetType().GetProperties().Where(p => !p.GetGetMethod().IsVirtual).Count();

         Assert.Equal(propertyCount, user.GetDatabaseFieldProperties().Count);
      }
   }
}
