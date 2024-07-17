using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
{
   public class RepositoryManager<T> : ObservableObject, IDisposable where T : ModelObject, new()
   {
      #region Fields

      private User _authenticatedUser;
      private ObservableCollection<ModelObject> m_SearchResults;
      private MultiTextFilter m_MultiTextFilter;  
      protected IRepository<T> _repository;

      #endregion

      #region Properties

      public ObservableCollection<ModelObject> SearchResults
      {
         get => m_SearchResults;
         set => SetProperty(ref m_SearchResults, value);
      }

      public MultiTextFilter MultiTextFilter
      {
         get => m_MultiTextFilter;
         set => SetProperty(ref m_MultiTextFilter, value);
      }

      public IRepository<T> Repository
      {
         get => _repository;
         set => SetProperty(ref _repository, value);
      }

      #endregion

      #region Constructor

      public RepositoryManager(IRepository<T> repository, User authenticatedUser)
      {
         _authenticatedUser = authenticatedUser;
         Repository = repository;
         SearchResults = new ObservableCollection<ModelObject>();
         MultiTextFilter = new MultiTextFilter(true);
      }

      #endregion

      #region Methods

      public async Task SearchAsync(CancellationToken cancellationToken)
      {
         Unload();

         var searchResults = await _repository.GetAllAsync(MultiTextFilter.StringFilters, MultiTextFilter.ExtendSearch, cancellationToken);

         foreach (var searchResult in searchResults)
         {
            SearchResults.Add(searchResult);
         }
      }

      protected virtual void Unload()
      {
         foreach (var searchResult in SearchResults)
         {
            searchResult.Dispose();
         }

         SearchResults.Clear();
      }

      public async Task<T> CreateAsync()
      {
         var modelObject = new T();
         SearchResults.Add(modelObject);

         await Repository.SaveAsync(modelObject, _authenticatedUser.Id.Value);

         return modelObject;
      }

      public async Task SaveAsync(ModelObject modelObject)
      {
         await Repository.SaveAsync(modelObject, _authenticatedUser.Id.Value);
         await Repository.LoadAsync(modelObject);
      }

      public async Task DeleteAsync(ModelObject modelObject)
      {
         await Repository.DeleteAsync(modelObject);
         SearchResults.Remove(modelObject);
      }

      public void Dispose()
      {
         Unload();
      }

      #endregion
   }
}
