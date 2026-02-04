using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Domain.Services;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Mapping;
using OtherSideCore.Infrastructure.Repositories;
using OtherSideCore.Infrastructure.Services;

namespace OtherSideCore.Infrastructure.Tests
{
   public class DatabaseFixture : IDisposable
   {
      public InfrastructureTestsDbContextFactory InfrastructureTestsDbContextFactory { get; }
      public Mock<ILoggerFactory> LoggerFactoryMock { get; }
      public Mock<ILogger> LoggerMock { get; }

      private PasswordService _passwordService;

      private IMapper _mapper;

      public DatabaseFixture()
      {
         InfrastructureTestsDbContextFactory = new InfrastructureTestsDbContextFactory();

         LoggerFactoryMock = new Mock<ILoggerFactory>();
         LoggerMock = new Mock<ILogger>();
         LoggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(LoggerMock.Object);

         var context = InfrastructureTestsDbContextFactory.CreateDbContext();
         context.Database.MigrateAsync().Wait();

         var config = new MapperConfiguration(cfg => cfg.AddProfile<GenericMappingProfile>());
         _mapper = config.CreateMapper();

         _passwordService = new PasswordService();

         SeedUsers();
      }

      public void SeedUsers()
      {
         var superAdmin = new Domain.DomainObjects.User
         {
            Id = 1,
            CreationDate = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            FirstName = "Super",
            LastName = "Admin",
            UserName = "administrator",
            PasswordHash = _passwordService.HashPassword("abcdefgh")
         };

         var anthony = new Domain.DomainObjects.User
         {
            Id = 2,
            CreationDate = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            FirstName = "Anthony",
            LastName = "Thonon",
            UserName = "anth",
            PasswordHash = _passwordService.HashPassword("abcdefgh")
         };

         var joy = new Domain.DomainObjects.User
         {
            Id = 3,
            CreationDate = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            FirstName = "Joy",
            LastName = "Malcourant",
            UserName = "joma",
            PasswordHash = _passwordService.HashPassword("abcdefgh")
         };

         var pierre = new Domain.DomainObjects.User
         {
            Id = 4,
            CreationDate = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            FirstName = "Pierre",
            LastName = "Malcourant",
            UserName = "pima",
            PasswordHash = _passwordService.HashPassword("abcdefgh")
         };

         var userRepository = new UserRepository(InfrastructureTestsDbContextFactory, _mapper, LoggerFactoryMock.Object);

         using (var context = InfrastructureTestsDbContextFactory.CreateDbContext())
         {
            var entity = _mapper.Map<Entities.User>(superAdmin);

            entity.CreationDate = DateTime.Now;
            entity.LastModifiedDateTime = DateTime.Now;

            context.Set<Entities.User>().Add(entity);
            context.SaveChanges();

            _mapper.Map(entity, superAdmin);
         }

         userRepository.CreateAsync(anthony, 1).Wait();
         userRepository.CreateAsync(joy, 1).Wait();
         userRepository.CreateAsync(pierre, 1).Wait();
      }

      public void Dispose()
      {
         InfrastructureTestsDbContextFactory.ReleaseInstance();
      }
   }

   public class UserRepository : Repository<Domain.DomainObjects.User, Entities.User>
   {
      public UserRepository(IDbContextFactory<DbContext> dbContextFactory, IMapper mapper, ILoggerFactory loggerFactory) : base(dbContextFactory, mapper, loggerFactory)
      {
      }

      public override Task<List<Domain.DomainObjects.User>> GetAllAsync(List<string> filters, List<Constraint<Domain.DomainObjects.User>> constraints, bool extendedSearch, CancellationToken cancellationToken = default)
      {
         throw new NotImplementedException();
      }
   }
}
