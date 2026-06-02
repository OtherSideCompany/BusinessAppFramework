using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   public interface IDomainObjectDocumentServiceFactory
   {
      IDomainObjectDocumentService<T> CreateDomainObjectDocumentService<T>() where T : DomainObject, new();
   }
}
