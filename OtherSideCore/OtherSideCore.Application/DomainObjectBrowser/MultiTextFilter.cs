using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter
{
   public class MultiTextFilter
   {
      #region Fields

     

      #endregion

      #region Properties

      public bool AllowExtendedSearch { get; private set; }

      public List<TextFilter> Filters { get; private set; }

      public bool ExtendedSearch { get; private set; }

      public List<string> StringFilters
      {
         get => Filters.Select(f => f.Text).ToList();
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MultiTextFilter(bool allowExtendedSearch = true)
      {
         Filters = new List<TextFilter>();
         AllowExtendedSearch = allowExtendedSearch;
      }

      #endregion

      #region Public Methods

      public void AddFilter(string searchText)
      {
         Filters.Add(new TextFilter(searchText));
      }

      public void RemoveFilter(TextFilter textFilter)
      {
         Filters.Remove(textFilter);
      }

      public void SetExtendedSearch(bool extendedSearch)
      {
         if (AllowExtendedSearch)
         {
            ExtendedSearch = extendedSearch;
         }
      }

      public void ClearFilters()
      {
         Filters.Clear();
      }

      #endregion
   }
}
