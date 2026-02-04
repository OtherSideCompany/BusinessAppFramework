using OtherSideCore.Infrastructure.Services;

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
         var passwordService = new PasswordService();
         var hashedPassword = passwordService.HashPassword(password);

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
         var passwordService = new PasswordService();
         var hashedPassword = passwordService.HashPassword(password);

         Assert.True(passwordService.VerifyPassword(hashedPassword, password));
      }
   }
}
