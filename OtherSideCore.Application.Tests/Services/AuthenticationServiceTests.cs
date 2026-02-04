using Moq;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure.Services;

namespace OtherSideCore.Domain.Tests.Services
{
   public class AuthenticationServiceTests
   {
      private AuthenticationService _authenticationService { get; }

      public AuthenticationServiceTests()
      {
         var passwordService = new PasswordService();

         User _anthony = new User();

         _anthony.Id = 2;
         _anthony.CreationDate = DateTime.Now;
         _anthony.CreatedBy = null;
         _anthony.LastModifiedDateTime = DateTime.Now;
         _anthony.LastModifiedBy = null;
         _anthony.FirstName = "Anthony";
         _anthony.LastName = "Thonon";
         _anthony.UserName = "anth";
         _anthony.PasswordHash = passwordService.HashPassword("abcdefgh");

         var userContext = new Mock<IUserContext>();
         userContext.Setup(x => x.Id).Returns(_anthony.Id);
         userContext.Setup(x => x.FirstName).Returns(_anthony.FirstName);
         userContext.Setup(x => x.LastName).Returns(_anthony.LastName);
         userContext.Setup(x => x.UserName).Returns(_anthony.UserName);
         userContext.Setup(x => x.IsInitialized).Returns(true);

         var userCredentialsRepository = new Mock<IUserCredentialsRepository>();
         userCredentialsRepository.Setup(x => x.GetUserPasswordHashAsync("anth")).ReturnsAsync((_anthony.Id, _anthony.PasswordHash));

         _authenticationService = new AuthenticationService(userCredentialsRepository.Object, passwordService);
      }

      public async Task VerifyPassword_ReturnsTrueIfPasswordIsCorrect()
      {
         (var result, var userId) = await _authenticationService.VerifyPasswordAsync("anth", "abcdefgh");
         Assert.True(result);
         Assert.Equal(2, userId);
      }
   }
}
