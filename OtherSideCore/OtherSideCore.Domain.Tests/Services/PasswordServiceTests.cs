using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.Services
{
   public class PasswordServiceTests
   {
      [Theory]
      [InlineData("")]
      [InlineData("test")]
      [InlineData("password")]
      [InlineData("sdmfksmofkqsùmf,zepofzkerpfokzprozkprékrpaok")]
      public void HashPassword_ReturnsHashedPassword(string password)
      {
         var hashedPassword = PasswordService.HashPassword(password);

         Assert.NotEqual(password, hashedPassword);
         Assert.Equal(64, hashedPassword.Length);
      }

      [Theory]
      [InlineData("")]
      [InlineData("test")]
      [InlineData("password")]
      [InlineData("sdmfksmofkqsùmf,zepofzkerpfokzprozkprékrpaok")]
      public void VerifyPassword_ReturnsTrueIfPasswordIsCorrect(string password)
      {
         var hashedPassword = PasswordService.HashPassword(password);

         Assert.True(PasswordService.VerifyPassword(hashedPassword, password));
      }
   }
}
