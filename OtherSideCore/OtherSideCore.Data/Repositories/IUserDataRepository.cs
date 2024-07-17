using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Repositories
{
   public interface IUserDataRepository<T> : IDataRepository<T> where T : Entities.User
   {
      Task<T> GetSuperAdminUserAsync();
   }
}
