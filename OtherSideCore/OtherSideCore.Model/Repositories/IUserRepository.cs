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
      Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      Task<T> GetSuperAdminUserAsync();

      Task LoadCreatorAndModificator(ModelObject modelObject, CancellationToken cancellationToken);
   }
}
