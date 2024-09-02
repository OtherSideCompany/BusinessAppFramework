using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;

namespace OtherSideCore.Domain.Repositories
{
    public interface IRepositorySearch<T> : IDisposable where T : ModelObject
   {
      MultiTextFilter MultiTextFilter { get; }

      ObservableCollection<ModelObject> SearchResults { get; }

      Task SearchAsync(CancellationToken cancellationToken);

      void AddSearchResult(T modelObject);

      void RemoveSearchResult(T modelObject);
   }
}
