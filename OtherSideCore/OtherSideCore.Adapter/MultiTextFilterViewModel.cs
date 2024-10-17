using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter
{
   public class MultiTextFilterViewModel : ObservableObject
   {
      #region Fields

      private MultiTextFilter m_MultiTextFilter;
      private AsyncRelayCommand _searchCommandAsync;
      private string _searchText;

      private bool _isInAdvancedMode;
      private ObservableCollection<TextFilterViewModel> _filterViewModels;

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

      public bool IsInAdvancedMode
      {
         get => _isInAdvancedMode;
         set => SetProperty(ref _isInAdvancedMode, value);
      }

      public ObservableCollection<TextFilterViewModel> FilterViewModels
      {
         get => _filterViewModels;
         set => SetProperty(ref _filterViewModels, value);
      }

      #endregion

      #region Events

      public event EventHandler SearchRequested;

      #endregion

      #region Commands

      public RelayCommand RequestSearchCommand { get; private set; }
      public RelayCommand RequestExtendedSearchCommand { get; private set; }
      public RelayCommand<TextFilterViewModel> RemoveFilterCommand { get; private set; }
      public RelayCommand ClearFiltersCommand { get; private set; }
      public RelayCommand ToggleModeCommand { get; private set; }
      public RelayCommand AddFilterCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter)
      {
         FilterViewModels = new ObservableCollection<TextFilterViewModel>();

         RequestSearchCommand = new RelayCommand(RequestSearch);
         RequestExtendedSearchCommand = new RelayCommand(RequestExtendedSearch);
         RemoveFilterCommand = new RelayCommand<TextFilterViewModel>((TextFilterViewModel textFilter) => FilterViewModels.Remove(textFilter));
         ClearFiltersCommand = new RelayCommand(() => FilterViewModels.Clear());
         ToggleModeCommand = new RelayCommand(ToggleMode);
         AddFilterCommand = new RelayCommand(() => FilterViewModels.Add(new TextFilterViewModel("Recherche...")));

         MultiTextFilter = multiTextFilter;
      }

      #endregion

      #region Public Methods


      #endregion

      #region Private Methods

      private void UpdateCommands()
      {
         RequestSearchCommand.NotifyCanExecuteChanged();
         RemoveFilterCommand.NotifyCanExecuteChanged();
      }

      private void RequestSearch()
      {
         MultiTextFilter.SetExtendedSearch(false);
         Search();
      }

      private void RequestExtendedSearch()
      {
         MultiTextFilter.SetExtendedSearch((true));
         Search();
      }

      private void Search()
      {
         MultiTextFilter.ClearFilters();

         if (IsInAdvancedMode)
         {
            FilterViewModels.ToList().ForEach(f => MultiTextFilter.AddFilter(f.FilterText));
         }
         else
         {
            MultiTextFilter.AddFilter(SearchText);
         }

         SearchRequested?.Invoke(this, EventArgs.Empty);
      }

      private void ToggleMode()
      {
         FilterViewModels.Clear();

         if (!IsInAdvancedMode)
         {
            FilterViewModels.Add(new TextFilterViewModel(String.IsNullOrEmpty(SearchText) ? "Recherche..." : SearchText));
         }

         SearchText = "";
         IsInAdvancedMode = !IsInAdvancedMode;
      }

      #endregion
   }
}
