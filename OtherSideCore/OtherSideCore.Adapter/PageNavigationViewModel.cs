using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OtherSideCore.Adapter
{
   public class PageNavigationViewModel : ObservableObject
   {
      #region Fields

      private int _pageCount;
      private int _resultsPerPage;
      private int _totalResults;
      private int _currentPageNumber;

      #endregion

      #region Properties

      public int ResultsPerPage
      {
         get => _resultsPerPage;
         set => SetProperty(ref _resultsPerPage, value);
      }

      public int TotalResults
      {
         get => _totalResults;
         set => SetProperty(ref _totalResults, value);
      }

      public int CurrentPageNumber
      {
         get => _currentPageNumber;
         set => SetProperty(ref _currentPageNumber, value);
      }

      public int MinResultIndex => (CurrentPageNumber - 1) * ResultsPerPage + 1;

      public int MaxResultIndex => CurrentPageNumber * ResultsPerPage > TotalResults ? TotalResults : CurrentPageNumber * ResultsPerPage;


      #endregion

      #region Commands
      public RelayCommand SetPreviousPageCommand { get; private set; }
      public RelayCommand SetNextPageCommand { get; private set; }
      public RelayCommand SetFirstPageCommand { get; private set; }
      public RelayCommand SetLastPageCommand { get; private set; }

      #endregion

      #region Constructor

      public PageNavigationViewModel()
      {
         SetPreviousPageCommand = new RelayCommand(() => SelectPage(Math.Max(CurrentPageNumber - 1, 1)));
         SetNextPageCommand = new RelayCommand(() => SelectPage(Math.Min(CurrentPageNumber + 1, _pageCount)));
         SetFirstPageCommand = new RelayCommand(() => SelectPage(1));
         SetLastPageCommand = new RelayCommand(() => SelectPage(_pageCount));
      }

      #endregion

      #region Public Methods

      public void SetPages(int pageCount, int resultsPerPage, int totalResutls)
      {
         _pageCount = pageCount;
         ResultsPerPage = resultsPerPage;
         TotalResults = totalResutls;

         SelectPage(1);
      }

      #endregion

      #region Private Methods

      private void SelectPage(int pageNumber)
      {
         CurrentPageNumber = pageNumber;
         OnPropertyChanged(nameof(MinResultIndex));
         OnPropertyChanged(nameof(MaxResultIndex));
      }

      #endregion
   }
}
