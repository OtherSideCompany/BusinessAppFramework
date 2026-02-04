using OtherSideCore.Application.Repository;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
   public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : DomainObject;
      object CreateRepository(Type type);
   }
}
