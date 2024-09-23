using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
   public interface IRepository<T> : IDisposable where T : ModelObject
   {
      Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

      Task<List<T>> GetAllAsync(List<string> filters, List<Constraint> constraints, bool extendedSearch, CancellationToken cancellationToken);

      Task<T> GetAsync(int id, CancellationToken cancellationToken);

      Task LoadAsync(T modelObject);

      Task<T> CreateAsync(int userId);

      Task SaveAsync(T modelObject, int userId);

      Task DeleteAsync(T modelObject);
   }
}
