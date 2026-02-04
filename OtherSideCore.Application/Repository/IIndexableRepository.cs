using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Repository
{
   public interface IIndexableRepository
   {
      int GetNewIndex(DomainObject domainObject);
   }
}
