using Microsoft.EntityFrameworkCore;
using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
    public interface IRepositoryFactory
   {
      IRepository<T> CreateRepository<T>() where T : ModelObject, new();

      IUserRepository<T> CreateUserRepository<T>() where T : User, new();
      
   }
}
