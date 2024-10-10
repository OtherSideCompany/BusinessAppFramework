using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Infrastructure.Tests.Repositories
{
   [Collection("Non-Parallel Tests")]
   public class MockRepositoryTests
   {
      private TestDomainObjectRepository _testDomainObjectRepository;
      private TestDomainObject _testDomainObject;

      public MockRepositoryTests()
      {
         _testDomainObject = new TestDomainObject();
         _testDomainObjectRepository = new TestDomainObjectRepository();
      }

      [Fact]
      public async Task CreateAsync_ShouldSetEntityId()
      {
         Assert.Equal(0, _testDomainObject.Id);

         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);

         Assert.True(_testDomainObject.Id > 0);
      }

      [Fact]
      public async Task GetAsync_EntityNotFound()
      {
         var entity = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);
         Assert.Null(entity);
      }

      [Fact]
      public async Task GetAsync_ModificationPersist()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);
         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id, CancellationToken.None);

         Assert.NotNull(_testDomainObject);

         var creationDate = _testDomainObject.CreationDate;
         var lastModifiedDateTime = _testDomainObject.LastModifiedDateTime;

         Assert.Equal(1, _testDomainObject.CreatedBy.Id);
         Assert.Equal(1, _testDomainObject.LastModifiedBy.Id);
         Assert.Equal(creationDate, _testDomainObject.CreationDate);
         Assert.Equal(lastModifiedDateTime, _testDomainObject.LastModifiedDateTime);
      }

      [Fact]
      public async Task SaveAsync_ModificationPersist()
      {
         await _testDomainObjectRepository.CreateAsync(_testDomainObject, 1);
         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id);

         var creationDate = _testDomainObject.CreationDate;

         await _testDomainObjectRepository.SaveAsync(_testDomainObject, 1);
         _testDomainObject = await _testDomainObjectRepository.GetAsync(_testDomainObject.Id);

         Assert.NotNull(_testDomainObject);
         Assert.NotEqual(_testDomainObject.CreationDate, default(DateTime));
         Assert.True(_testDomainObject.LastModifiedDateTime > creationDate);
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
         Assert.Equal(_testDomainObject.Id, 0);
         Assert.Equal(_testDomainObject.CreatedBy, null);
         Assert.Equal(_testDomainObject.LastModifiedBy, null);
         Assert.Equal(_testDomainObject.CreationDate, default);
         Assert.Equal(_testDomainObject.LastModifiedDateTime, default);
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

      protected class TestDomainObjectRepository : MockRepository<TestDomainObject>
      {
         public TestDomainObjectRepository() : base()
         {

         }
      }
   }
}
