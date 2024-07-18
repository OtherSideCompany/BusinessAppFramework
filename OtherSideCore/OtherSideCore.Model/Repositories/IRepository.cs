using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
{
    public interface IRepository<T> : IDisposable where T : ModelObject
   {
      Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      Task<T> GetAsync(int id, CancellationToken cancellationToken);

      Task LoadAsync(T modelObject);

      Task<T> CreateAsync(int userId);

      Task SaveAsync(T modelObject, int userId);

      Task DeleteAsync(T modelObject);
   }
}
