using Microsoft.EntityFrameworkCore;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
{
    public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : ModelObject, new();

      IRepository<T> CreateSpecificRepository<T>() where T : ModelObject, new();

      IUserRepository<T> CreateUserRepository<T>() where T : User, new();
      
   }
}
