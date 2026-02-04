using Domain.DomainObjects;

namespace Application.Interfaces
{
   public interface IReferenceHydrator
   {
      Task HydrateAsync(DomainObject domainObject);
   }
}
