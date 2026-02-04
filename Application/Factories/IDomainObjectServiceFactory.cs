using Application.Services;
using Domain.DomainObjects;

namespace Application.Factories
{
   public interface IDomainObjectServiceFactory
   {
      IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
      object CreateDomainObjectService(Type domainObjectType);
   }
}
