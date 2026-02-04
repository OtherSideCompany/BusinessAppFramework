using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IReferenceHydrator
   {
      Task HydrateAsync(DomainObject domainObject);
   }
}
