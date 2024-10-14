using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public abstract class DomainObjectServiceFactory : IDomainObjectServiceFactory
   {
      protected IRepositoryFactory _repositoryFactory;
      protected IUserContext _userContext;
      protected IGlobalDataService _globalDataService;

      public DomainObjectServiceFactory(IRepositoryFactory repositoryFactory, IUserContext userContext, IGlobalDataService globalDataService)
      {
         _repositoryFactory = repositoryFactory;
         _userContext = userContext;
         _globalDataService = globalDataService;
      }

      public abstract IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
   }
}
