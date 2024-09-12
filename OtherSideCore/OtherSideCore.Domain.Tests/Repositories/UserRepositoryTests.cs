using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class UserRepositoryTests
   {
      private UserRepository<User, Infrastructure.Entities.User> _userRepository { get; }

      Infrastructure.Entities.User _anthony = new Infrastructure.Entities.User
      {
         Id = 2,
         CreationDate = DateTime.Now,
         CreatedById = 1,
         LastModifiedDateTime = DateTime.Now,
         LastModifiedById = 1,
         FirstName = "Anthony",
         LastName = "Thonon",
         UserName = "anth",
         PasswordHash = PasswordService.HashPassword("abcdefgh")
      };

      public UserRepositoryTests()
      {
         var userDataRepository = new Mock<UserDataRepository<Infrastructure.Entities.User>>(new Mock<IDbContextFactory<DbContext>>().Object, new Mock<Microsoft.Extensions.Logging.ILoggerFactory>().Object);

         userDataRepository.Setup(x => x.GetUserPasswordHashAsync("anth")).ReturnsAsync((_anthony.Id, _anthony.PasswordHash));

         _userRepository = new UserRepository<User, Infrastructure.Entities.User>(userDataRepository.Object, new ModelObjectFactory());
      }

      [Fact]
      async Task GetUserByCredentials_UserIsReturned()
      {
         var (id, passwordHash) = await _userRepository.GetUserPasswordHashAsync("anth");
         
         Assert.Equal(2, id);
         Assert.NotNull(passwordHash);
         Assert.NotEqual("abcdefgh", passwordHash);
      }

      [Fact]
      async Task GetUserByCredentials_UserIsNotReturnedIfWrongPassword()
      {
         var (id, passwordHash) = await _userRepository.GetUserPasswordHashAsync("test");

         Assert.Equal(0, id);
         Assert.True(string.IsNullOrEmpty(passwordHash));
      }
   }
}
