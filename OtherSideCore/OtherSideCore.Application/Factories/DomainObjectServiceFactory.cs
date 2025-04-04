using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Factories
{
   public abstract class DomainObjectServiceFactory : IDomainObjectServiceFactory
   {
      protected IRepositoryFactory _repositoryFactory;
      protected IUserContext _userContext;
      protected IDomainObjectEventPublisher _domainObjectEventPublisher;

      public IDomainObjectEventPublisher DomainObjectEventPublisher => _domainObjectEventPublisher;

      public DomainObjectServiceFactory(
         IRepositoryFactory repositoryFactory, 
         IUserContext userContext)
      {
         _repositoryFactory = repositoryFactory;
         _userContext = userContext;

         CreateDomainObjectEventPublisher();
      }     

      public abstract IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();

      public abstract object CreateDomainObjectService(Type type);

      protected abstract void CreateDomainObjectEventPublisher();
   }
}
