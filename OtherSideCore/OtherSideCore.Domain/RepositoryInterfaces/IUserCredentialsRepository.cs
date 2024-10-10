using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.RepositoryInterfaces
{
   public interface IUserCredentialsRepository
   {
      Task<(int userId, string passwordHash)> GetUserPasswordHashAsync(string userName);
   }
}
