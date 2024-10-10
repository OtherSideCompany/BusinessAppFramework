using AutoMapper;
using Moq;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Repositories;
using OtherSideCore.Infrastructure.Services;

namespace OtherSideCore.Application.Tests.Services
{
   public class DomainObjectServiceTests
   {
      private DomainObjectService<DefaultDomainObject> _domainObjectService { get; }

      public DomainObjectServiceTests()
      {
         var passwordService = new PasswordService();

         Infrastructure.Entities.User _anthony = new Infrastructure.Entities.User();

         _anthony.Id = 1;
         _anthony.CreationDate = DateTime.Now;
         _anthony.CreatedBy = null;
         _anthony.LastModifiedDateTime = DateTime.Now;
         _anthony.LastModifiedBy = null;
         _anthony.FirstName = "Anthony";
         _anthony.LastName = "Thonon";
         _anthony.UserName = "anth";
         _anthony.PasswordHash = passwordService.HashPassword("abcdefgh");

         var repository = new DefaultEntityRepository();

         var userContext = new Mock<IUserContext>();
         userContext.Setup(x => x.Id).Returns(_anthony.Id);
         userContext.Setup(x => x.FirstName).Returns(_anthony.FirstName);
         userContext.Setup(x => x.LastName).Returns(_anthony.LastName);
         userContext.Setup(x => x.UserName).Returns(_anthony.UserName);
         userContext.Setup(x => x.IsInitialized).Returns(true);

         _domainObjectService = new DomainObjectService<DefaultDomainObject>(repository, userContext.Object);
      }

      [Fact]
      public async Task CreateAsync_ModelObjectIsCreated()
      {
         var defaultModelObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultModelObject);

         Assert.True(defaultModelObject.Id > 0);
         Assert.Equal(1, defaultModelObject.CreatedBy.Id);
         Assert.Equal(1, defaultModelObject.LastModifiedBy.Id);
         Assert.NotEqual(defaultModelObject.CreationDate, default);
         Assert.NotEqual(defaultModelObject.LastModifiedDateTime, default);
      }

      [Fact]
      public async Task GetAsync_EntityNotFound()
      {
         var defaultModelObject = await _domainObjectService.GetAsync(12, CancellationToken.None);
         Assert.Null(defaultModelObject);
      }

      [Fact]
      public async Task GetAsync_ModificationPersist()
      {
         var defaultModelObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultModelObject);

         var creationDate = defaultModelObject.CreationDate;
         var lastModifiedDateTime = defaultModelObject.LastModifiedDateTime;

         defaultModelObject = await _domainObjectService.GetAsync(defaultModelObject.Id, CancellationToken.None);

         Assert.NotNull(defaultModelObject);
         Assert.Equal(1, defaultModelObject.CreatedBy.Id);
         Assert.Equal(1, defaultModelObject.LastModifiedBy.Id);
         Assert.Equal(creationDate, defaultModelObject.CreationDate);
         Assert.Equal(lastModifiedDateTime, defaultModelObject.LastModifiedDateTime);
      }

      [Fact]
      public async Task SaveAsync_ModificationPersist()
      {
         var defaultModelObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultModelObject);

         defaultModelObject.RandomProperty = "test";

         await _domainObjectService.SaveAsync(defaultModelObject);
         defaultModelObject = await _domainObjectService.GetAsync(defaultModelObject.Id, CancellationToken.None);

         Assert.NotNull(defaultModelObject);
         Assert.Equal("test", defaultModelObject.RandomProperty);
      }

      [Fact]
      public async Task DeleteAsync_EntityIsDeleted()
      {
         var defaultModelObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultModelObject);

         Assert.NotEqual(0, defaultModelObject.Id);

         await _domainObjectService.DeleteAsync(defaultModelObject);

         Assert.Equal(0, defaultModelObject.Id);
         Assert.Null(defaultModelObject.CreatedBy);
         Assert.Null(defaultModelObject.LastModifiedBy);
         Assert.Equal(default, defaultModelObject.CreationDate);
         Assert.Equal(default, defaultModelObject.LastModifiedDateTime);
      }

      [Fact]
      public async Task LoadAsync_PropertiesAreReloaded()
      {
         var defaultModelObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultModelObject);
         var id = defaultModelObject.Id;

         defaultModelObject = new DefaultDomainObject() { Id = id };
         defaultModelObject.RandomProperty = "test";

         await _domainObjectService.LoadAsync(defaultModelObject);

         Assert.Equal(GlobalVariables.DefaultString, defaultModelObject.RandomProperty);
      }
   }
}
