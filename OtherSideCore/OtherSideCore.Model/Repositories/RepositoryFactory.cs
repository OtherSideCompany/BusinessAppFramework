using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Data.Repositories;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
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
         if (typeof(T) == typeof(User))
         {
            return CreateUserRepository<User>() as IRepository<T>;
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

      public virtual IUserRepository<T> CreateUserRepository<T>() where T : User, new()
      {
         var userDataRepository = new UserDataRepository<Data.Entities.User>(_dbContextFactory, _loggerFactory);
         return new UserRepository<T, Data.Entities.User>(userDataRepository, _modelObjectFactory);
      }
   }
}
