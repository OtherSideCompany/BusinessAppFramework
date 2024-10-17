using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;

namespace OtherSideCore.Adapter
{
   public class PageNavigationViewModel : ObservableObject
   {
      #region Fields

      private PageNavigation _pageNavigation;

      #endregion

      #region Properties    

      public PageNavigation PageNavigation
      {
         get => _pageNavigation;
         set => SetProperty(ref _pageNavigation, value);
      }

      public int TotalResults => PageNavigation.TotalResults;

      public int MinResultIndex => PageNavigation.MinResultIndex;

      public int MaxResultIndex => PageNavigation.MaxResultIndex;

      #endregion

      #region Events

      public event EventHandler SelectPageRequested;

      #endregion

      #region Commands
      public RelayCommand SetPreviousPageCommand { get; private set; }
      public RelayCommand SetNextPageCommand { get; private set; }
      public RelayCommand SetFirstPageCommand { get; private set; }
      public RelayCommand SetLastPageCommand { get; private set; }
      public RelayCommand SelectPageCommand { get; private set; }

      #endregion

      #region Constructor

      public PageNavigationViewModel(PageNavigation pageNavigation)
      {
         PageNavigation = pageNavigation;

         SetPreviousPageCommand = new RelayCommand(() => { PageNavigation.SelectPreviousPage(); UpdateProperties(); PageRequested(); });
         SetNextPageCommand = new RelayCommand(() => { PageNavigation.SelectNextPage(); UpdateProperties(); PageRequested(); });
         SetFirstPageCommand = new RelayCommand(() => { PageNavigation.SelectFirstPage(); UpdateProperties(); PageRequested(); });
         SetLastPageCommand = new RelayCommand(() => { PageNavigation.SelectLastPage(); UpdateProperties(); PageRequested(); });
      }

      #endregion

      #region Public Methods

      public void Refresh()
      {
         UpdateProperties();
      }

      #endregion

      #region Private Methods

      private void PageRequested()
      {
         SelectPageRequested?.Invoke(this, EventArgs.Empty);
      }

      private void UpdateProperties()
      {
         OnPropertyChanged(nameof(TotalResults));
         OnPropertyChanged(nameof(MinResultIndex));
         OnPropertyChanged(nameof(MaxResultIndex));
      }

      #endregion
   }
}
