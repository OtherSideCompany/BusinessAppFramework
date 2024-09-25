using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class DefaultModelObjectRepository<T, U> : Repository<T, U>, IRepository<T> where T : DefaultModelObject, new()
                                                                                      where U : DefaultEntity, new()
   {
      public DefaultModelObjectRepository(IDataRepository<U> repository, IModelObjectFactory modelObjectFactory, IGlobalDataService globalDataService) : base(repository, modelObjectFactory, globalDataService)
      {

      }
   }
}
