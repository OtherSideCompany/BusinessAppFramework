using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public interface IUserDataRepository<T> : IDataRepository<T> where T : User
   {
      Task<T> GetSuperAdminUserAsync();
   }
}
