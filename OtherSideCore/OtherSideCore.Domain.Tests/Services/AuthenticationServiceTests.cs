using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Domain.Tests.Services
{
   public class AuthenticationServiceTests
   {
      private AuthenticationService<User> _authenticationService { get; }

      public AuthenticationServiceTests()
      {
         User _anthony = new User();

         _anthony.Id.Value = 2;
         _anthony.CreationDate.Value = DateTime.Now;
         _anthony.CreatedById.Value = 1;
         _anthony.LastModifiedDateTime.Value = DateTime.Now;
         _anthony.LastModifiedById.Value = 1;
         _anthony.FirstName.Value = "Anthony";
         _anthony.LastName.Value = "Thonon";
         _anthony.UserName.Value = "anth";
         _anthony.PasswordHash.Value = PasswordService.HashPassword("abcdefgh");

         var mockDbContextFactory = new Mock<IDbContextFactory<DbContext>>();

         var mockLogger = new Mock<ILoggerFactory>();

         var mockGlobalDataService = new Mock<IGlobalDataService>();

         var modelObjectFactory = new ModelObjectFactory();

         var repositoryFactory = new Mock<RepositoryFactory>(mockDbContextFactory.Object, modelObjectFactory, mockLogger.Object, mockGlobalDataService.Object);
         var userRepository = new Mock<IUserRepository<User>>();
         userRepository.Setup(x => x.GetUserPasswordHashAsync("anth")).ReturnsAsync((_anthony.Id.Value, _anthony.PasswordHash.Value));
         userRepository.Setup(x => x.GetAsync(2, CancellationToken.None)).ReturnsAsync(_anthony);

         repositoryFactory.Setup(x => x.CreateUserRepository<User>()).Returns(userRepository.Object);

         _authenticationService = new AuthenticationService<User>(repositoryFactory.Object);
      }

      [Fact]
      public void CanAuthenticateUser_ReturnsTrueWhenServiceIsInitialized()
      {
         Assert.True(_authenticationService.CanAuthenticateUser("user", "pwdHash"));
      }

      [Theory]
      [InlineData("","")]
      [InlineData(null, null)]
      [InlineData("test", "")]
      [InlineData("test", null)]
      [InlineData("", "test")]
      [InlineData(null, "test")]
      public void CanAuthenticateUser_ReturnsFalseWhenArgsAreNullOrEmpty(string username, string passwordHash)
      {
         Assert.False(_authenticationService.CanAuthenticateUser(username, passwordHash));
      }

      [Fact]
      public async Task AuthenticateUserAsync_UserIsAuthenticated()
      {
         await _authenticationService.AuthenticateUserAsync("anth", "abcdefgh");

         Assert.True(_authenticationService.IsUserAuthenticated());
         Assert.False(_authenticationService.CanAuthenticateUser("anth", "abcdefgh"));
      }

      [Fact]
      public async Task AuthenticateUserAsync_UserCanBeLoggedOutAfterAuthentication()
      {
         await _authenticationService.AuthenticateUserAsync("anth", "abcdefgh");

         Assert.True(_authenticationService.CanLogoutUser());

         await _authenticationService.LogoutUserAsync();

         Assert.False(_authenticationService.CanLogoutUser());
         Assert.True(_authenticationService.CanAuthenticateUser("test", "abcdefgh"));
      }
   }
}
