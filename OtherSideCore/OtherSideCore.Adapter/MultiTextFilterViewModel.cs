using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OtherSideCore.Adapter
{
   public class MultiTextFilterViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private MultiTextFilter m_MultiTextFilter;
      private AsyncRelayCommand _searchCommandAsync;
      private string _searchText;
      private bool _extendedSearch;

      #endregion

      #region Properties

      public MultiTextFilter MultiTextFilter
      {
         get => m_MultiTextFilter;
         set => SetProperty(ref m_MultiTextFilter, value);
      }

      public string SearchText
      {
         get => _searchText;
         set
         {
            SetProperty(ref _searchText, value);
            UpdateCommands();
         }
      }

      public bool ExtendedSearch
      {
         get => _extendedSearch;
         set => SetProperty(ref _extendedSearch, value);
      }

      #endregion

      #region Events

      public event EventHandler SearchRequested;

      #endregion

      #region Commands

      public RelayCommand RequestSearchCommand { get; private set; }

      public AsyncRelayCommand<TextFilter> RemoveFilterAsyncCommand { get; private set; }

      public AsyncRelayCommand ClearFiltersAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter)
      {
         RequestSearchCommand = new RelayCommand(RequestSearch);
         RemoveFilterAsyncCommand = new AsyncRelayCommand<TextFilter>(RemoveFilterAsync, CanRemoveFilter);
         ClearFiltersAsyncCommand = new AsyncRelayCommand(ClearFiltersAsync);

         MultiTextFilter = multiTextFilter;
      }


      #endregion

      #region Public Methods

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods

      private void UpdateCommands()
      {
         RequestSearchCommand.NotifyCanExecuteChanged();
         RemoveFilterAsyncCommand.NotifyCanExecuteChanged();
      }

      private void RequestSearch()
      {
         MultiTextFilter.AddFilter(SearchText);

         SearchRequested?.Invoke(this, EventArgs.Empty);
         SearchText = "";
      }

      private bool CanRemoveFilter(TextFilter textFilter)
      {
         return textFilter != null;
      }

      private async Task RemoveFilterAsync(TextFilter textFilter)
      {
         MultiTextFilter.RemoveFilter(textFilter);
      }

      private async Task ClearFiltersAsync()
      {
         MultiTextFilter.ClearFilters();
      }

      #endregion
   }
}
