using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         get
         {
            return m_Filters;
         }
         set
         {
            if (value != m_Filters)
            {
               m_Filters = value;
               OnPropertyChanged(nameof(Filters));
            }
         }
      }

      public bool ExtendedSearch
      {
         get
         {
            return m_ExtendedSearch;
         }
         set
         {
            if (m_ExtendedSearch != value)
            {
               m_ExtendedSearch = value;
               OnPropertyChanged(nameof(ExtendedSearch));
            }
         }
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
