using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OtherSideCore.Model
{
   public class MultiTextFilter : ObservableObject
   {
      #region Fields

      private bool m_AllowExtendedSearch;
      private bool m_ExtendSearch;      

      private ObservableCollection<TextFilter> m_Filters;

      #endregion

      #region Properties

      public bool AllowExtendedSearch
      {
         get => m_AllowExtendedSearch;
         set => SetProperty(ref m_AllowExtendedSearch, value);
      }

      public bool ExtendSearch
      {
         get => m_ExtendSearch;
         set => SetProperty(ref m_ExtendSearch, value);
      }

      public ObservableCollection<TextFilter> Filters
      {
         get => m_Filters;
         set => SetProperty(ref m_Filters, value);
      }

      public List<string> StringFilters
      {
         get => Filters.Select(f => f.Text).ToList();
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MultiTextFilter(bool allowExtendedSearch = false)
      {
         Filters = new ObservableCollection<TextFilter>();
         AllowExtendedSearch = allowExtendedSearch;
      }

      #endregion

      #region Methods

      public void AddFilter()
      {
         Filters.Add(new TextFilter("Nouveau filtre"));
      }

      public void RemoveFilter(TextFilter textFilter)
      {
         Filters.Remove(textFilter);
      }



      #endregion
   }
}
