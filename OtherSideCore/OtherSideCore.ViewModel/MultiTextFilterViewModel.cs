using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtherSideCore.ViewModel
{
   public class MultiTextFilterViewModel : ViewModelBase
   {
      #region Fields

      private MultiTextFilter m_MultiTextFilter;
      private AsyncRelayCommand<bool> _searchCommandAsync;
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

      public AsyncRelayCommand<bool> SearchCommandAsync
      {
         get => _searchCommandAsync;
         set => SetProperty(ref _searchCommandAsync, value);
      }

      #endregion

      #region Commands

      public AsyncRelayCommand AddFilterAndSearchAsyncCommand { get; private set; }

      public AsyncRelayCommand<TextFilter> RemoveFilterAsyncCommand { get; private set; }

      public AsyncRelayCommand ClearFiltersAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter, AsyncRelayCommand<bool> searchCommandAsync)
      {
         AddFilterAndSearchAsyncCommand = new AsyncRelayCommand(AddFilterAndSearchAsync, CanAddFilterAndSearch);
         RemoveFilterAsyncCommand = new AsyncRelayCommand<TextFilter>(RemoveFilterAsync, CanRemoveFilter);
         ClearFiltersAsyncCommand = new AsyncRelayCommand(ClearFiltersAsync);

         SearchCommandAsync = searchCommandAsync;

         MultiTextFilter = multiTextFilter;
      }


      #endregion

      #region Public Methods

      public override void Dispose()
      {
         
      }

      #endregion

      #region Private Methods

      private void UpdateCommands()
      {
         AddFilterAndSearchAsyncCommand.NotifyCanExecuteChanged();
         RemoveFilterAsyncCommand.NotifyCanExecuteChanged();
      }

      private bool CanAddFilterAndSearch()
      {
         return !String.IsNullOrEmpty(SearchText);
      }

      private async Task AddFilterAndSearchAsync()
      {
         MultiTextFilter.AddFilter(SearchText);

         if (SearchCommandAsync.CanExecute(ExtendedSearch))
         {
            await SearchCommandAsync.ExecuteAsync(ExtendedSearch);
            SearchText = "";
         }
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
