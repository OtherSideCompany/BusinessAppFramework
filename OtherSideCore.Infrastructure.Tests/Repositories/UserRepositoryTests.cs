using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
   [Collection("Non-Parallel Tests")]
   public class UserRepositoryTests : IClassFixture<DatabaseFixture>
   {
      private readonly DatabaseFixture _databaseFixture;
      private UserCredentialsRepository<Entities.User> _userRepository;

      public UserRepositoryTests(DatabaseFixture databaseFixture)
      {
         _databaseFixture = databaseFixture;
         _userRepository = new UserCredentialsRepository<Entities.User>(_databaseFixture.InfrastructureTestsDbContextFactory);
      }

      [Fact]
      async Task GetUserPasswordHashAsync_PasswordHashIsReturned()
      {
         var (id, passwordHash) = await _userRepository.GetUserPasswordHashAsync("anth");

         Assert.Equal(2, id);
         Assert.NotNull(passwordHash);
         Assert.NotEqual("abcdefgh", passwordHash);
      }

      [Fact]
      async Task GetUserPasswordHashAsync_UserIsNotReturnedIfNotExists()
      {
         var (id, passwordHash) = await _userRepository.GetUserPasswordHashAsync("test");

         Assert.Equal(0, id);
         Assert.True(string.IsNullOrEmpty(passwordHash));
      }
   }
}
