using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.Tests.Repositories;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
   [Collection("Non-Parallel Tests")]
   public class MockRepositoryTests
   {  
      private TestEntityRepository _testEntityRepository;
      private TestEntity _testEntity;

      public MockRepositoryTests()
      {
         _testEntity = new TestEntity();
         _testEntityRepository = new TestEntityRepository();
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
         _testEntity = await _testEntityRepository.GetAsync(_testEntity.Id, CancellationToken.None) as TestEntity;

         Assert.NotNull(_testEntity);

         var creationDate = _testEntity.CreationDate;
         var lastModifiedDateTime = _testEntity.LastModifiedDateTime;
         
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

      protected class TestEntityRepository : MockDataRepository<TestEntity>
      {
         public TestEntityRepository() : base()
         {
         }
      }
   }   
}
