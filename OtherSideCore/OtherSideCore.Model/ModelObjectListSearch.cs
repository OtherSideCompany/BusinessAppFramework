using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public abstract class ModelObjectListSearch : ObservableObject, IDisposable
   {
      #region Fields

      private bool m_IsSelectionLocked;
      private ObservableCollection<ModelObject> m_SearchResults;
      private MultiTextFilter m_MultiTextFilter;
      private ModelObject m_SelectedModelObject;

      private Func<Task> m_SelectedSearchResultChangedAsync;

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

      public Func<Task> SelectedSearchResultChangedAsync
      {
         get => m_SelectedSearchResultChangedAsync;
         set => SetProperty(ref m_SelectedSearchResultChangedAsync, value);
      }

      #endregion

      #region Constructor

      public ModelObjectListSearch()
      {
         SearchResults = new ObservableCollection<ModelObject>();
         MultiTextFilter = new MultiTextFilter(true);
      }

      #endregion

      #region Methods

      public virtual async Task SearchAsync()
      {
         Unload();
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

      public async virtual Task SelectModelObjectAsync(ModelObject modelObject)
      {
         SelectedModelObject = modelObject;
         await SelectedSearchResultChangedAsync?.Invoke();
      }

      public void Dispose()
      {
         Unload();
      }      

      #endregion
   }
}
