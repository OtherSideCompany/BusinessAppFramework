using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public abstract class DomainObjectQueryServiceFactory : IDomainObjectQueryServiceFactory
   {
      protected IRepositoryFactory _repositoryFactory;

      public DomainObjectQueryServiceFactory(IRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
      }

      public abstract IDomainObjectQueryService<T> CreateDomainObjectQueryService<T>() where T : DomainObject, new();
   }
}
