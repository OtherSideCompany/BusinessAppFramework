using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
    public interface IUserRepository<T> : IRepository<T> where T : User
   {
      Task LoadCreatorAndModificator(ModelObject modelObject, CancellationToken cancellationToken);

      Task<(int, string)> GetUserPasswordHashAsync(string userName);
   }
}
