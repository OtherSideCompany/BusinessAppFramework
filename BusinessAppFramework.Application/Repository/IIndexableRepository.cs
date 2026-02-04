using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Repository
{
   public interface IIndexableRepository
   {
      int GetNewIndex(DomainObject domainObject);
   }
}
