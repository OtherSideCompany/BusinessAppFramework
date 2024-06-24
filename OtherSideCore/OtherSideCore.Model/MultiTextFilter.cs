using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace OtherSideCore.Model
{
   public class MultiTextFilter : ObservableObject
   {
      #region Fields

      public bool m_ExtendedSearch;

      private ObservableCollection<TextFilter> m_Filters;

      #endregion

      #region Properties

      public ObservableCollection<TextFilter> Filters
      {
         get => m_Filters;
         set => SetProperty(ref m_Filters, value);
      }

      public bool ExtendedSearch
      {
         get => m_ExtendedSearch;
         set => SetProperty(ref m_ExtendedSearch, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MultiTextFilter()
      {
         Filters = new ObservableCollection<TextFilter>();
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
