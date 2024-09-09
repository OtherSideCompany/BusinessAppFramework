using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;

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
         _anthony.PasswordHash.Value = "abcdefgh";

         var mockDbContextFactory = new Mock<IDbContextFactory<DbContext>>();

         var mockLogger = new Mock<ILoggerFactory>();

         var modelObjectFactory = new ModelObjectFactory();

         var repositoryFactory = new Mock<RepositoryFactory>(mockDbContextFactory.Object, modelObjectFactory, mockLogger.Object);
         var userRepository = new Mock<IUserRepository<User>>();
         userRepository.Setup(x => x.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_anthony);

         repositoryFactory.Setup(x => x.CreateUserRepository<User>()).Returns(userRepository.Object);

         _authenticationService = new AuthenticationService<User>(repositoryFactory.Object);
      }

      [Fact]
      public void CanAuthenticateUser_ReturnsTrueWhenServiceIsInitialized()
      {
         Assert.True(_authenticationService.CanAuthenticateUser());
      }

      [Fact]
      public async Task AuthenticateUserAsync_UserIsAuthenticated()
      {
         await _authenticationService.AuthenticateUserAsync("test", "test");

         Assert.NotNull(_authenticationService.AuthenticatedUser);
         Assert.False(_authenticationService.CanAuthenticateUser());
      }

      [Fact]
      public async Task AuthenticateUserAsync_UserCanBeLoggedOutAfterAuthentication()
      {
         await _authenticationService.AuthenticateUserAsync("test", "test");

         Assert.True(_authenticationService.CanLogoutUser());

         await _authenticationService.LogoutUserAsync();

         Assert.False(_authenticationService.CanLogoutUser());
         Assert.True(_authenticationService.CanAuthenticateUser());
      }
   }
}
