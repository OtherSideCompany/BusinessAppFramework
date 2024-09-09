using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests
{
   public class DatabaseFixture : IDisposable
   {
      public InfrastructureTestsDbContextFactory InfrastructureTestsDbContextFactory { get; }
      public Mock<ILoggerFactory> LoggerFactoryMock { get; }
      public Mock<ILogger> LoggerMock { get; }

      public DatabaseFixture()
      {
         InfrastructureTestsDbContextFactory = new InfrastructureTestsDbContextFactory();

         LoggerFactoryMock = new Mock<ILoggerFactory>();
         LoggerMock = new Mock<ILogger>();
         LoggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(LoggerMock.Object);

         var context = InfrastructureTestsDbContextFactory.CreateDbContext();
         context.Database.MigrateAsync().Wait();

         SeedUsers();
      }

      public void SeedUsers()
      {
         var superAdmin = new User
         {
            Id = 1,
            CreationDate = DateTime.Now,
            CreatedById = 1,
            LastModifiedDateTime = DateTime.Now,
            LastModifiedById = 1,
            IsSuperAdmin = true,
            FirstName = "Super",
            LastName = "Admin",
            UserName = "superadmin"
         };

         var anthony = new User
         {
            Id = 2,
            CreationDate = DateTime.Now,
            CreatedById = 1,
            LastModifiedDateTime = DateTime.Now,
            LastModifiedById = 1,
            IsSuperAdmin = false,
            FirstName = "Anthony",
            LastName = "Thonon",
            UserName = "anth"
         };

         var joy = new User
         {
            Id = 3,
            CreationDate = DateTime.Now,
            CreatedById = 1,
            LastModifiedDateTime = DateTime.Now,
            LastModifiedById = 2,
            IsSuperAdmin = false,
            FirstName = "Joy",
            LastName = "Malcourant",
            UserName = "joma"
         };

         var pierre = new User
         {
            Id = 4,
            CreationDate = DateTime.Now,
            CreatedById = 1,
            LastModifiedDateTime = DateTime.Now,
            LastModifiedById = 2,
            IsSuperAdmin = false,
            FirstName = "Pierre",
            LastName = "Malcourant",
            UserName = "pima"
         };

         var userRepository = new UserDataRepository<User>(InfrastructureTestsDbContextFactory, LoggerFactoryMock.Object);

         userRepository.CreateAsync(superAdmin.GetDatabaseFieldProperties()).Wait();
         userRepository.CreateAsync(anthony.GetDatabaseFieldProperties()).Wait();
         userRepository.CreateAsync(joy.GetDatabaseFieldProperties()).Wait();
         userRepository.CreateAsync(pierre.GetDatabaseFieldProperties()).Wait();
      }

      public void Dispose()
      {
         InfrastructureTestsDbContextFactory.ReleaseInstance();
      }
   }
}
