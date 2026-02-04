using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : DomainObject;
      object CreateRepository(Type type);
   }
}
