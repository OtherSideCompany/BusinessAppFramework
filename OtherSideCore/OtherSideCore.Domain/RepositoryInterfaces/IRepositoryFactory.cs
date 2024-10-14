using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Domain.RepositoryInterfaces
{
   public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : DomainObject, new();
   }
}
