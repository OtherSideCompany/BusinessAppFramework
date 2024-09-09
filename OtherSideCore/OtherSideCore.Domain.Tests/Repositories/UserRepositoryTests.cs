using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
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
         PasswordHash = "abcdefgh"
      };

      public UserRepositoryTests()
      {
         var userDataRepository = new Mock<UserDataRepository<Infrastructure.Entities.User>>(new Mock<IDbContextFactory<DbContext>>().Object, new Mock<Microsoft.Extensions.Logging.ILoggerFactory>().Object);

         userDataRepository.Setup(x => x.GetUserByCredentials("anth", "abcdefgh")).ReturnsAsync(_anthony);

         _userRepository = new UserRepository<User, Infrastructure.Entities.User>(userDataRepository.Object, new ModelObjectFactory());
      }

      [Fact]
      async Task GetUserByCredentials_UserIsReturned()
      {
         var user = await _userRepository.GetUserByCredentials("anth", "abcdefgh");

         Assert.NotNull(user);
         Assert.Equal(2, user.Id.Value);
      }

      [Fact]
      async Task GetUserByCredentials_UserIsNotReturnedIfWrongPassword()
      {
         var user = await _userRepository.GetUserByCredentials("anth", "kuhlkihliuh");

         Assert.Null(user);
      }
   }
}
