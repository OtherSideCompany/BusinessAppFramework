using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class RepositoryTests
   {
      private DefaultModelObjectRepository<DefaultModelObject, DefaultEntity> _defaultModelObjectRepository { get; }

      public RepositoryTests()
      {
         var defaultEntityDataRepository = new DefaultEntityDataRepository();
         _defaultModelObjectRepository = new DefaultModelObjectRepository<DefaultModelObject, DefaultEntity>(defaultEntityDataRepository, new ModelObjectFactory());
      }

      [Fact]
      public async Task CreateAsync_ModelObjectIsCreated()
      {
         var defaultModelObject = await _defaultModelObjectRepository.CreateAsync(1);

         Assert.True(defaultModelObject.Id.Value > 0);
         Assert.Equal(1, defaultModelObject.CreatedById.Value);
         Assert.Equal(1, defaultModelObject.LastModifiedById.Value);         
      }

      [Fact]
      public async Task GetAsync_EntityNotFound()
      {
         var defaultModelObject = await _defaultModelObjectRepository.GetAsync(12, CancellationToken.None);
         Assert.Null(defaultModelObject);
      }

      [Fact]
      public async Task GetAsync_ModificationPersist()
      {
         var defaultModelObject = await _defaultModelObjectRepository.CreateAsync(1);

         var creationDate = defaultModelObject.CreationDate;
         var lastModifiedDateTime = defaultModelObject.LastModifiedDateTime;

         defaultModelObject = await _defaultModelObjectRepository.GetAsync(defaultModelObject.Id.Value, CancellationToken.None);

         Assert.NotNull(defaultModelObject);        
         Assert.Equal(1, defaultModelObject.CreatedById.Value);
         Assert.Equal(1, defaultModelObject.LastModifiedById.Value);
         Assert.Equal(creationDate.Value, defaultModelObject.CreationDate.Value);
         Assert.Equal(lastModifiedDateTime.Value, defaultModelObject.LastModifiedDateTime.Value);
      }

      [Fact]
      public async Task SaveAsync_ModificationPersist()
      {
         var defaultModelObject = await _defaultModelObjectRepository.CreateAsync(1);
         defaultModelObject = await _defaultModelObjectRepository.GetAsync(defaultModelObject.Id.Value, CancellationToken.None);

         var creationDate = defaultModelObject.CreationDate.Value;
         var lastModificationDate = defaultModelObject.LastModifiedDateTime.Value;

         await _defaultModelObjectRepository.SaveAsync(defaultModelObject, 2);
         defaultModelObject = await _defaultModelObjectRepository.GetAsync(defaultModelObject.Id.Value, CancellationToken.None);

         Assert.NotNull(defaultModelObject);
         Assert.Equal(2, defaultModelObject.LastModifiedById.Value);
         Assert.Equal(creationDate, defaultModelObject.CreationDate.Value);
         Assert.True(defaultModelObject.LastModifiedDateTime.Value > lastModificationDate);
      }

      [Fact]
      public async Task SaveAsync_EntityCreatedIfNotExists()
      {
         var defaultModelObject = new DefaultModelObject();
         await _defaultModelObjectRepository.SaveAsync(defaultModelObject, 1);
         defaultModelObject = await _defaultModelObjectRepository.GetAsync(defaultModelObject.Id.Value, CancellationToken.None);

         Assert.NotNull(defaultModelObject);
      }

      [Fact]
      public async Task DeleteAsync_EntityIsDeleted()
      {
         var defaultModelObject = await _defaultModelObjectRepository.CreateAsync(1);

         Assert.NotEqual(0, defaultModelObject.Id.Value);

         await _defaultModelObjectRepository.DeleteAsync(defaultModelObject);

         Assert.Equal(0, defaultModelObject.Id.Value);
      }

      [Fact]
      public async Task LoadAsync_PropertiesAreReloaded()
      {
         var defaultModelObject = await _defaultModelObjectRepository.CreateAsync(1);

         var creationDate = defaultModelObject.CreationDate.Value;
         var lastModificationDate = defaultModelObject.LastModifiedDateTime.Value;

         defaultModelObject.LastModifiedDateTime.Value = DateTime.Now.AddMinutes(7);
         defaultModelObject.CreationDate.Value = DateTime.Now.AddMinutes(7);
         defaultModelObject.LastModifiedById.Value = 2;
         defaultModelObject.CreatedById.Value = 3;

         await _defaultModelObjectRepository.LoadAsync(defaultModelObject);

         Assert.Equal(creationDate, defaultModelObject.CreationDate.Value);
         Assert.Equal(lastModificationDate, defaultModelObject.LastModifiedDateTime.Value);
         Assert.Equal(1, defaultModelObject.CreatedById.Value);
         Assert.Equal(1, defaultModelObject.LastModifiedById.Value);
      }

      [Fact]
      public async Task GetAll_AllObjectsAreReturned()
      {
         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);

         var modelobjects = await _defaultModelObjectRepository.GetAllAsync(CancellationToken.None);

         Assert.Equal(4, modelobjects.Count);
      }

      [Fact]
      public async Task GetAllWithFilter_AllObjectsAreReturned()
      {
         var modelObject = await _defaultModelObjectRepository.CreateAsync(1);
         modelObject.RandomProperty.Value = "test";
         await _defaultModelObjectRepository.SaveAsync(modelObject, 1);

         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);

         var modelobjects = await _defaultModelObjectRepository.GetAllAsync(new List<string>() { "test" }, [], false, CancellationToken.None);

         Assert.Single(modelobjects);
      }

      [Fact]
      public async Task GetAllWithConstrained_ConstrainedObjectsAreReturned()
      {
         var modelObject = await _defaultModelObjectRepository.CreateAsync(1);
         modelObject.RandomProperty.Value = "test";
         await _defaultModelObjectRepository.SaveAsync(modelObject, 1);

         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);
         await _defaultModelObjectRepository.CreateAsync(1);

         var constraints = new List<Constraint>();
         constraints.Add(new Constraint("Test constraint", nameof(DefaultModelObject.RandomProperty), "test"));

         var modelobjects = await _defaultModelObjectRepository.GetAllAsync([], constraints, false, CancellationToken.None);

         Assert.Single(modelobjects);
      }
   }
}
