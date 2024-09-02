using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure.Repositories;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Domain.ModelObjects;
using System;

namespace OtherSideCore.Domain.Repositories
{
   public class RepositoryFactory : IRepositoryFactory
   {
      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected IModelObjectFactory _modelObjectFactory;
      protected ILoggerFactory _loggerFactory;

      public RepositoryFactory(IDbContextFactory<DbContext> dbContextFactory, IModelObjectFactory modelObjectFactory, ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _modelObjectFactory = modelObjectFactory;
         _loggerFactory = loggerFactory;
      }

      public virtual IRepository<T> CreateRepository<T>() where T : ModelObject, new()
      {
         if (typeof(T) == typeof(ModelObjects.User))
         {
            return CreateUserRepository<ModelObjects.User>() as IRepository<T>;
         }
         else
         {
            return CreateSpecificRepository<T>();            
         }
      }

      public virtual IRepository<T> CreateSpecificRepository<T>() where T : ModelObject, new()
      {
         throw new ArgumentException("Unknown repository type", typeof(T).ToString());
      }

      public virtual IUserRepository<T> CreateUserRepository<T>() where T : ModelObjects.User, new()
      {
         var userDataRepository = new UserDataRepository<Infrastructure.Entities.User>(_dbContextFactory, _loggerFactory);
         return new UserRepository<T, Infrastructure.Entities.User>(userDataRepository, _modelObjectFactory);
      }
   }
}
