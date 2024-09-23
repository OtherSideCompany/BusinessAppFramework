using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
   [Collection("Non-Parallel Tests")]
   public class DataRepositoryTests : IClassFixture<DatabaseFixture>
   {
      private readonly DatabaseFixture _databaseFixture;      

      private TestEntityRepository _testEntityRepository;
      private TestEntity _testEntity;

      public DataRepositoryTests(DatabaseFixture databaseFixture)
      {
         _databaseFixture = databaseFixture;

         _testEntity = new TestEntity();
         _testEntityRepository = new TestEntityRepository(_databaseFixture.InfrastructureTestsDbContextFactory, _databaseFixture.LoggerFactoryMock.Object);
      }

      [Fact]
      public async Task CreateAsync_ShouldReturnEntityId()
      {      
         Assert.Equal(0, _testEntity.Id);

         _testEntity.Id = await _testEntityRepository.CreateAsync(_testEntity.GetDatabaseFieldProperties());

         Assert.True(_testEntity.Id > 0);
      }

      [Fact]
      public async Task GetAsync_EntityNotFound()
      {
         var entity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None);
         Assert.Null(entity);
      }

      [Fact]
      public async Task GetAsync_ModificationPersist()
      {
         _testEntity.Id = await _testEntityRepository.CreateAsync(_testEntity.GetDatabaseFieldProperties());

         var creationDate = _testEntity.CreationDate;
         var lastModifiedDateTime = _testEntity.LastModifiedDateTime;

         _testEntity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None) as TestEntity;

         Assert.NotNull(_testEntity);
         Assert.Equal(1, _testEntity.CreatedById);
         Assert.Equal(1, _testEntity.LastModifiedById);
         Assert.Equal(creationDate, _testEntity.CreationDate);
         Assert.Equal(lastModifiedDateTime, _testEntity.LastModifiedDateTime);
      }

      [Fact]
      public async Task SaveAsync_ModificationPersist()
      {     
         _testEntity.Id = await _testEntityRepository.CreateAsync(_testEntity.GetDatabaseFieldProperties());
         _testEntity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None) as TestEntity;

         var futureDateTime = DateTime.Now.AddMinutes(7);

         _testEntity.CreationDate = futureDateTime;
         _testEntity.LastModifiedDateTime = futureDateTime;

         await _testEntityRepository.SaveAsync(_testEntity.Id, _testEntity.GetDatabaseFieldProperties());
         _testEntity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None) as TestEntity;

         Assert.NotNull(_testEntity);
         Assert.Equal(_testEntity.CreationDate, futureDateTime);
         Assert.Equal(_testEntity.LastModifiedDateTime, futureDateTime);
      }

      [Fact]
      public async Task SaveAsync_EntityDoesNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testEntityRepository.SaveAsync(_testEntity.Id, _testEntity.GetDatabaseFieldProperties()));
      }

      [Fact]
      public async Task DeleteAsync_EntityDoNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testEntityRepository.DeleteAsync(_testEntity.Id));
      }

      [Fact]
      public async Task DeleteAsync_EntityIsDeleted()
      {
         _testEntity.Id = await _testEntityRepository.CreateAsync(_testEntity.GetDatabaseFieldProperties());
         await _testEntityRepository.DeleteAsync(_testEntity.Id);

         var entity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None);

         Assert.Null(entity);
      }

      [Fact]
      public async Task GetModificationTimeAsync_EntityDoNotExists()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testEntityRepository.GetModificatonTimeAsync(_testEntity.Id));
      }

      [Fact]
      public async Task GetModificationTimeAsync_ModificationPersist()
      {
         _testEntity.Id = await _testEntityRepository.CreateAsync(_testEntity.GetDatabaseFieldProperties());

         var futureModificationTime = DateTime.Now.AddMinutes(5);

         _testEntity.LastModifiedDateTime = futureModificationTime;

         await _testEntityRepository.SaveAsync(_testEntity.Id, _testEntity.GetDatabaseFieldProperties());
         var modificationTime = await _testEntityRepository.GetModificatonTimeAsync(_testEntity.Id);

         Assert.Equal(modificationTime, futureModificationTime);
      }

      protected class TestEntityRepository : DataRepository<TestEntity>
      {
         public TestEntityRepository(IDbContextFactory<DbContext> dbContextFactory, ILoggerFactory loggerFactory) : base(dbContextFactory, loggerFactory)
         {
         }

         public override Task<List<TestEntity>> GetAllAsync(List<string> filters, List<Constraint<TestEntity>> constraints, bool extendedSearch, CancellationToken cancellationToken)
         {
            throw new NotImplementedException();
         }
      }
   }   
}
