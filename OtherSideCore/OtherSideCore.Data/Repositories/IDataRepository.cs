using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Repositories
{
   public interface IDataRepository<T> : IDisposable
   {
      Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken);

      Task<int> CreateAsync(List<DatabaseField> databaseFields);

      Task SaveAsync(int entityId, List<DatabaseField> databaseFields);

      Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken);

      Task DeleteAsync(int entityId);
   }
}
