using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   public interface IDomainObjectServiceFactory
   {
      IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
      object CreateDomainObjectService(Type domainObjectType);
   }
}
