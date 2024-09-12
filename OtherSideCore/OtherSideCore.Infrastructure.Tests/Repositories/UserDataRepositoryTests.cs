using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
   [Collection("Non-Parallel Tests")]
   public class UserDataRepositoryTests : IClassFixture<DatabaseFixture>
   {
      private readonly DatabaseFixture _databaseFixture;
      private UserDataRepository<User> _userRepository;

      public UserDataRepositoryTests(DatabaseFixture databaseFixture)
      {
         _databaseFixture = databaseFixture;
         _userRepository = new UserDataRepository<User>(_databaseFixture.InfrastructureTestsDbContextFactory, _databaseFixture.LoggerFactoryMock.Object);
      }

      [Fact]
      public async Task GetAllAsync_ReturnAllUsers()
      {
         var users = await _userRepository.GetAllAsync(null, false, CancellationToken.None);

         Assert.Equal(4, users.Count);
      }

      [Fact]
      public async Task GetAllWithTextFiltersAsync_ReturnUsersSubset()
      {
         var users = await _userRepository.GetAllAsync(new List<string> { "Malcourant" }, false, CancellationToken.None);

         Assert.Equal(2, users.Count);
      }

      [Fact]
      public async Task GetAllWithTextFiltersAndExtendedSearchAsync_ReturnUsersSubset()
      {
         var users = await _userRepository.GetAllAsync(new List<string> { "Malcpurant" }, true, CancellationToken.None);

         Assert.Equal(2, users.Count);
      }

      [Fact] async Task GetUserPasswordHashAsync_PasswordHashIsReturned()
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
