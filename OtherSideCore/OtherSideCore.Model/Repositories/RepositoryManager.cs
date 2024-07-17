using CommunityToolkit.Mvvm.ComponentModel;
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

      private bool m_IsSelectionLocked;
      private ObservableCollection<ModelObject> m_SearchResults;
      private MultiTextFilter m_MultiTextFilter;
      private ModelObject m_SelectedModelObject;

      private Func<CancellationToken, Task> m_SelectedSearchResultChangedAsync;

      protected IRepository<T> _repository;

      #endregion

      #region Properties

      public bool IsSelectionLocked
      {
         get => m_IsSelectionLocked;
         set => SetProperty(ref m_IsSelectionLocked, value);
      }

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

      public ModelObject SelectedModelObject
      {
         get => m_SelectedModelObject;
         set => SetProperty(ref m_SelectedModelObject, value);
      }

      public Func<CancellationToken, Task> SelectedSearchResultChangedAsync
      {
         get => m_SelectedSearchResultChangedAsync;
         set => SetProperty(ref m_SelectedSearchResultChangedAsync, value);
      }

      public IRepository<T> Repository
      {
         get => _repository;
         set => SetProperty(ref _repository, value);
      }

      #endregion

      #region Constructor

      public RepositoryManager(IRepository<T> repository)
      {
         Repository = repository;
         SearchResults = new ObservableCollection<ModelObject>();
         MultiTextFilter = new MultiTextFilter(true);
      }

      #endregion

      #region Methods

      public T CreateModelObjectInstance()
      {
         return new T();
      }

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

      public void LockSelection()
      {
         IsSelectionLocked = true;
      }

      public void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      public bool CanSelectModelObject(ModelObject modelObject)
      {
         return !IsSelectionLocked &&
                modelObject != null &&
                !modelObject.Equals(SelectedModelObject);
      }

      public void UnselectModelObject()
      {
         SelectedModelObject = null;
      }

      public async virtual Task SelectModelObjectAsync(ModelObject modelObject, CancellationToken cancellationToken)
      {
         SelectedModelObject = modelObject;

         if (SelectedSearchResultChangedAsync != null)
         {
            await SelectedSearchResultChangedAsync(cancellationToken);
         }
      }

      public void RemoveSearchResult(ModelObject modelObject)
      {
         if (SelectedModelObject.Equals(modelObject))
         {
            UnselectModelObject();
         }

         SearchResults.Remove(modelObject);
      }

      public void Dispose()
      {
         Unload();
      }

      #endregion
   }
}
