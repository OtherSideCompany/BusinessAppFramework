using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

      private Action m_SelectedSearchResultChanged;

      #endregion

      #region Properties

      public bool IsSelectionLocked
      {
         get
         {
            return m_IsSelectionLocked;
         }
         set
         {
            if (value != m_IsSelectionLocked)
            {
               m_IsSelectionLocked = value;
               OnPropertyChanged(nameof(IsSelectionLocked));
            }
         }
      }

      public ObservableCollection<ModelObject> SearchResults
      {
         get
         {
            return m_SearchResults;
         }
         set
         {
            if (value != m_SearchResults)
            {
               m_SearchResults = value;
               OnPropertyChanged(nameof(SearchResults));
            }
         }
      }

      public MultiTextFilter MultiTextFilter
      {
         get
         {
            return m_MultiTextFilter;
         }
         set
         {
            if (value != m_MultiTextFilter)
            {
               m_MultiTextFilter = value;
               OnPropertyChanged(nameof(MultiTextFilter));
            }
         }
      }

      public ModelObject SelectedModelObject
      {
         get
         {
            return m_SelectedModelObject;
         }
         set
         {
            if (value != m_SelectedModelObject)
            {
               m_SelectedModelObject = value;
               OnPropertyChanged(nameof(SelectedModelObject));
            }
         }
      }

      public Action SelectedSearchResultChanged
      {
         get
         {
            return m_SelectedSearchResultChanged;
         }
         set
         {
            if (value != m_SelectedSearchResultChanged)
            {
               m_SelectedSearchResultChanged = value;
               OnPropertyChanged(nameof(SelectedSearchResultChanged));
            }
         }
      }

      #endregion



      #region Constructor

      public ModelObjectListSearch()
      {
         SearchResults = new ObservableCollection<ModelObject>();
         MultiTextFilter = new MultiTextFilter();
      }

      #endregion

      #region Methods

      public virtual void Search()
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

      public void SelectModelObject(ModelObject modelObject)
      {
         SelectedModelObject = modelObject;
         SelectedSearchResultChanged?.Invoke();
      }

      public void Dispose()
      {
         Unload();
      }

      

      #endregion
   }
}
