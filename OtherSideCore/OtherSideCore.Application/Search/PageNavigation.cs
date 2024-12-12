using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Search
{
    public class PageNavigation
    {
        #region Fields



        #endregion

        #region Properties

        public int PageCount { get; private set; }

        public int ResultsPerPage { get; private set; }

        public int TotalResults { get; private set; }

        public int CurrentPageNumber { get; private set; }

        public int MinResultIndex => (CurrentPageNumber - 1) * ResultsPerPage + 1;

        public int MaxResultIndex => CurrentPageNumber * ResultsPerPage > TotalResults ? TotalResults : CurrentPageNumber * ResultsPerPage;


        #endregion

        #region Constructor

        public PageNavigation()
        {

        }

        #endregion

        #region Public Methods

        public void SetPages(int pageCount, int resultsPerPage, int totalResutls)
        {
            PageCount = pageCount;
            ResultsPerPage = resultsPerPage;
            TotalResults = totalResutls;
        }

        public void SelectPreviousPage()
        {
            SelectPage(CurrentPageNumber - 1);
        }

        public void SelectNextPage()
        {
            SelectPage(CurrentPageNumber + 1);
        }

        public void SelectFirstPage()
        {
            SelectPage(1);
        }

        public void SelectLastPage()
        {
            SelectPage(PageCount);
        }

        #endregion

        #region Private Methods

        private void SelectPage(int pageNumber)
        {
            if (pageNumber < 1)
            {
                CurrentPageNumber = 1;
            }
            else if (pageNumber > PageCount)
            {
                pageNumber = PageCount;
            }
            else
            {
                CurrentPageNumber = pageNumber;
            }
        }

        #endregion
    }
}
