using AutoMapper;
using Moq;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Services;

namespace OtherSideCore.Application.Tests.Services
{
   public class DomainObjectQueryServiceTests
   {
      private DomainObjectService<DefaultDomainObject> _domainObjectService { get; }
      private DomainObjectQueryService<DefaultDomainObject> _domainObjectQueryService { get; }

      public DomainObjectQueryServiceTests()
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

         _domainObjectQueryService = new DomainObjectQueryService<DefaultDomainObject>(repository);
         _domainObjectService = new DomainObjectService<DefaultDomainObject>(repository, userContext.Object);

         var defaultModelObject1 = new DefaultDomainObject(); defaultModelObject1.RandomProperty = "sapin";
         var defaultModelObject2 = new DefaultDomainObject(); defaultModelObject2.RandomProperty = "pastèque";
         var defaultModelObject3 = new DefaultDomainObject(); defaultModelObject3.RandomProperty = "truite";
         var defaultModelObject4 = new DefaultDomainObject(); defaultModelObject4.RandomProperty = "chaussette";

         _domainObjectService.CreateAsync(defaultModelObject1).Wait();
         _domainObjectService.CreateAsync(defaultModelObject2).Wait();
         _domainObjectService.CreateAsync(defaultModelObject3).Wait();
         _domainObjectService.CreateAsync(defaultModelObject4).Wait();
      }

      [Fact]
      public async Task GetAll_AllObjectsAreReturned()
      {
         await _domainObjectQueryService.SearchAsync([], []);

         Assert.Equal(4, _domainObjectQueryService.Results.Count);
      }

      [Fact]
      public async Task SearchWithFilter_AllObjectsAreReturned()
      {
         var defaultDomainObject = new DefaultDomainObject();
         await _domainObjectService.CreateAsync(defaultDomainObject);

         defaultDomainObject.RandomProperty = "test";
         await _domainObjectService.SaveAsync(defaultDomainObject);

         await _domainObjectService.CreateAsync(new DefaultDomainObject());
         await _domainObjectService.CreateAsync(new DefaultDomainObject());
         await _domainObjectService.CreateAsync(new DefaultDomainObject());

         await _domainObjectQueryService.SearchAsync(new List<string>() { "test" }, [], false, CancellationToken.None);

         Assert.Single(_domainObjectQueryService.Results);
      }

      [Fact]
      public async Task Dispose_NoSearchResultAreStored()
      {
         await _domainObjectQueryService.SearchAsync([], [], false, CancellationToken.None);
         _domainObjectQueryService.Dispose();

         Assert.Empty(_domainObjectQueryService.Results);
      }

      [Fact]
      public async Task AddSearchResult_ResultIsAdded()
      {
         await _domainObjectQueryService.SearchAsync([], [], false, CancellationToken.None);

         var defaultModelObject = new DefaultDomainObject();
         _domainObjectQueryService.AddResult(defaultModelObject);

         Assert.Equal(5, _domainObjectQueryService.Results.Count);
      }

      [Fact]
      public async Task RemoveSearchResult_ResultIsRemove()
      {
         await _domainObjectQueryService.SearchAsync([], [], false, CancellationToken.None);

         _domainObjectQueryService.RemoveResult(_domainObjectQueryService.Results.First());

         Assert.Equal(3, _domainObjectQueryService.Results.Count);
      }

      [Fact]
      public async Task GetConstrain_ResultIsRemove()
      {
         await _domainObjectQueryService.SearchAsync([], [], false, CancellationToken.None);

         _domainObjectQueryService.RemoveResult(_domainObjectQueryService.Results.First());

         Assert.Equal(3, _domainObjectQueryService.Results.Count);
      }

      //[Fact]
      //public async Task GetAllWithConstrained_ConstrainedObjectsAreReturned()
      //{
      //   var defaultModelObject = new DefaultDomainObject();
      //   await _domainObjectService.CreateAsync(defaultModelObject);

      //   modelObject.RandomProperty = "test";
      //   await _domainObjectService.SaveAsync(modelObject, 1);

      //   await _domainObjectService.CreateAsync(1);
      //   await _domainObjectService.CreateAsync(1);
      //   await _domainObjectService.CreateAsync(1);

      //   var constraints = new List<Constraint>();
      //   constraints.Add(new Constraint("Test constraint", nameof(DefaultDomainObject.RandomProperty), "test"));

      //   var modelobjects = await _domainObjectService.GetAllAsync([], constraints, false, CancellationToken.None);

      //   Assert.Single(modelobjects);
      //}
   }
}
