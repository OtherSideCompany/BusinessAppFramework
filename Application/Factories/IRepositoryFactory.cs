using Application.Repository;
using Domain.DomainObjects;

namespace Application.Factories
{
   public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : DomainObject;
      object CreateRepository(Type type);
   }
}
