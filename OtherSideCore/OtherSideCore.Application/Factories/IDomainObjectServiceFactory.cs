using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
   public interface IDomainObjectServiceFactory
   {
      void RegisterDomainObjectService<T>(Func<IDomainObjectService<T>> factory) where T : DomainObject, new();
      IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
      object CreateDomainObjectService(Type type);
   }
}
