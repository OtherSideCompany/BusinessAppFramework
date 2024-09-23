using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
    public class RepositorySearch<T> : ObservableObject, IRepositorySearch<T> where T : ModelObject, new()
   {
      #region Fields

      private ObservableCollection<ModelObject> m_SearchResults;
      protected IRepository<T> _repository;

      #endregion

      #region Properties

      public ObservableCollection<ModelObject> SearchResults
      {
         get => m_SearchResults;
         set => SetProperty(ref m_SearchResults, value);
      }

      public IRepository<T> Repository
      {
         get => _repository;
         set => SetProperty(ref _repository, value);
      }

      #endregion

      #region Constructor

      public RepositorySearch(IRepository<T> repository)
      {
         Repository = repository;
         SearchResults = new ObservableCollection<ModelObject>();
      }

      #endregion

      #region Public Methods

      public async Task SearchAsync(List<string> filters, List<Constraint> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         Unload();

         var searchResults = await _repository.GetAllAsync(filters, constraints, extendedSearch, cancellationToken);

         foreach (var searchResult in searchResults)
         {
            AddSearchResult(searchResult);
         }
      }      

      public void AddSearchResult(T modelObject)
      {
         SearchResults.Add(modelObject);
      }

      public void RemoveSearchResult(T modelObject)
      {
         SearchResults.Remove(modelObject);
      }

      public void Dispose()
      {
         Unload();
      }

      #endregion

      #region Private Methods

      protected virtual void Unload()
      {
         foreach (var searchResult in SearchResults)
         {
            searchResult.Dispose();
         }

         SearchResults.Clear();
      }

      #endregion
   }
}
