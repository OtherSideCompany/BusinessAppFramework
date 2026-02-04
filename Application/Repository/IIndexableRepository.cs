using Domain.DomainObjects;

namespace Application.Repository
{
   public interface IIndexableRepository
   {
      int GetNewIndex(DomainObject domainObject);
   }
}
