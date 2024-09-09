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

         Assert.NotEqual("UserFirstName", user.FirstName);
         Assert.NotEqual("UserLastName", user.LastName);
         Assert.NotEqual("UserUserName", user.UserName);

         var databaseFields = new List<DatabaseField>
               {
                   new StringDatabaseField("UserFirstName", nameof(User.FirstName)),
                   new StringDatabaseField("UserLastName", nameof(User.LastName)),
                   new StringDatabaseField("UserUserName", nameof(User.UserName))
               };

         user.SetProperties(databaseFields);

         Assert.Equal("UserFirstName", user.FirstName);
         Assert.Equal("UserLastName", user.LastName);
         Assert.Equal("UserUserName", user.UserName);
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
