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
      ObservableCollection<ModelObject> SearchResults { get; }

      Task SearchAsync(List<string> filters, List<Constraint> constraints, bool extendedSearch, CancellationToken cancellationToken);

      void AddSearchResult(T modelObject);

      void RemoveSearchResult(T modelObject);
   }
}
