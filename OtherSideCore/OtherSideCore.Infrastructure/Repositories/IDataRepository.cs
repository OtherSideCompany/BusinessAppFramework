using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public interface IDataRepository<T> : IDisposable where T : EntityBase
   {
      Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

      Task<List<T>> GetAllAsync(List<string> filters, List<Constraint<T>> constraints, bool extendedSearch, CancellationToken cancellationToken);

      Task<int> CreateAsync(List<DatabaseField> databaseFields);

      Task SaveAsync(int entityId, List<DatabaseField> databaseFields);

      Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken);

      Task DeleteAsync(int entityId);

      Task<DateTime> GetModificatonTimeAsync(int entityId);
   }
}
