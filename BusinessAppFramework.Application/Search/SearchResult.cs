namespace BusinessAppFramework.Application.Search
{
   public class SearchResult<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields



      #endregion

      #region Properties

      public List<TSearchResult> Items { get; set; } = new()!;
      public int Count { get; set; }

      #endregion

      #region Events



      #endregion

      #region Constructor

      public SearchResult()
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
