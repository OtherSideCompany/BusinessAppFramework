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
      Task<T> GetAsync(int id, CancellationToken cancellationToken);

      Task LoadAsync(ModelObject modelObject);

      Task SaveAsync(ModelObject modelObject, int userId);

      Task DeleteAsync(ModelObject modelObject);

      void Dispose();
   }
}
