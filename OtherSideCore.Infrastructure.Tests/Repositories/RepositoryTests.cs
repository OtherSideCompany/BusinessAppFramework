using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure.Mapping;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
    [Collection("Non-Parallel Tests")]
   public class RepositoryTests : IClassFixture<DatabaseFixture>
   {
      private readonly DatabaseFixture _databaseFixture;

      private TestDomainObjectRepository _testDomainObjectRepository;
      private TestDomainObject _testDomainObject;

      public RepositoryTests(DatabaseFixture databaseFixture)
      {
         _databaseFixture = databaseFixture;

         var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
         var mapper = config.CreateMapper();

         _testDomainObject = new TestDomainObject();
         _testDomainObjectRepository = new TestDomainObjectRepository(_databaseFixture.InfrastructureTestsDbContextFactory, mapper, _databaseFixture.LoggerFactoryMock.Object);
      }

      [Fact]
      public async Task CreateAsync_ShouldSetEntityId()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);

         Assert.True(_testDomainObject.Id > 0);
         Assert.NotEqual(_testDomainObject.CreationDate, default);
         Assert.NotEqual(_testDomainObject.LastModifiedDateTime, default);
         Assert.NotNull(_testDomainObject.CreatedBy);
         Assert.NotNull(_testDomainObject.LastModifiedBy);
      }

      [Fact]
      public async Task GetAsync_EntityNotFound()
      {
         var domainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);
         Assert.Null(domainObject);
      }

      [Fact]
      public async Task GetAsync_ModificationPersist()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);

         var creationDate = _testDomainObject.CreationDate;
         var lastModifiedDateTime = _testDomainObject.LastModifiedDateTime;

         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);

         Assert.NotNull(_testDomainObject);
         Assert.Equal(1, _testDomainObject.CreatedBy.Id);
         Assert.Equal(1, _testDomainObject.LastModifiedBy.Id);
         Assert.Equal(creationDate, _testDomainObject.CreationDate);
         Assert.Equal(lastModifiedDateTime, _testDomainObject.LastModifiedDateTime);
      }

      [Fact]
      public async Task SaveAsync_ModificationPersist()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);
         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);

         var creationDate = _testDomainObject.CreationDate;

         await _testDomainObjectRepository.SaveAsync(_testDomainObject, 2);
         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);

         Assert.NotNull(_testDomainObject);
         Assert.NotEqual(_testDomainObject.CreationDate, default(DateTime));
         Assert.True(_testDomainObject.LastModifiedDateTime > creationDate);
         Assert.True(_testDomainObject.Id > 0);
         Assert.Equal(1, _testDomainObject.CreatedBy.Id);
         Assert.Equal(2, _testDomainObject.LastModifiedBy.Id);
      }

      [Fact]
      public async Task SaveAsync_EntityDoesNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testDomainObjectRepository.SaveAsync(_testDomainObject, 1));
      }

      [Fact]
      public async Task DeleteAsync_EntityDoNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testDomainObjectRepository.DeleteAsync(_testDomainObject));
      }

      [Fact]
      public async Task DeleteAsync_EntityIsDeleted()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);
         await _testDomainObjectRepository.DeleteAsync(_testDomainObject);

         var entity = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);

         Assert.Null(entity);
         Assert.Equal(0, _testDomainObject.Id);
         Assert.Null(_testDomainObject.CreatedBy);
         Assert.Null(_testDomainObject.LastModifiedBy);
         Assert.Equal(default, _testDomainObject.CreationDate);
         Assert.Equal(default, _testDomainObject.LastModifiedDateTime);
      }

      [Fact]
      public async Task GetModificationTimeAsync_EntityDoNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testDomainObjectRepository.GetLastModificatonTimeAsync(_testDomainObject));
      }

      [Fact]
      public async Task GetModificationTimeAsync_ModificationPersist()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);

         var creationDate = _testDomainObject.CreationDate;

         await _testDomainObjectRepository.SaveAsync(_testDomainObject, 1);
         var modificationTime = await _testDomainObjectRepository.GetLastModificatonTimeAsync(_testDomainObject);

         Assert.True(modificationTime > creationDate);
      }

      [Fact]
      public async Task GetAllAsync_ReturnAllUsers()
      {
         await _testDomainObjectRepository.CreateAsync(new TestDomainObject(), 1);
         await _testDomainObjectRepository.CreateAsync(new TestDomainObject(), 1);
         await _testDomainObjectRepository.CreateAsync(new TestDomainObject(), 1);
         await _testDomainObjectRepository.CreateAsync(new TestDomainObject(), 1);

         var domainObjects = await _testDomainObjectRepository.GetAllAsync(CancellationToken.None);

         Assert.Equal(4, domainObjects.Count);
      }

      protected class TestDomainObjectRepository : Repository<TestDomainObject, TestEntity>
      {
         public TestDomainObjectRepository(IDbContextFactory<DbContext> dbContextFactory, IMapper mapper, ILoggerFactory loggerFactory) : base(dbContextFactory, mapper, loggerFactory)
         {
         }

         public override Task<List<TestDomainObject>> GetAllAsync(List<string> filters, List<Constraint<TestDomainObject>> constraints, bool extendedSearch, CancellationToken cancellationToken = default)
         {
            throw new NotImplementedException();
         }
      }
   }
}
