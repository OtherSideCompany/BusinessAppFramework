using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Search
{
    public class SearchResult<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields



        #endregion

        #region Properties

        public IEnumerable<TSearchResult> Items { get; set; }
        public int Count { get; set; }
        public List<DomainObjectSearchResult> DomainObjectSearchResults { get; set; } = new()!;

        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        //public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

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
