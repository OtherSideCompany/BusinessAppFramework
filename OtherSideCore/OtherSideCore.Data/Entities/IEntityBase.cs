using OtherSideCore.Data.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Data.Entities
{
   internal interface IEntityBase
   {
      List<DatabaseField> GetDatabaseFieldProperties();

      Task<int> CreateAsync(List<DatabaseField> databaseFields);

      Task SaveAsync(int entityId, List<DatabaseField> databaseFields);

      Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken);

      Task DeleteAsync(int entityId);
   }
}
