using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class DomainObjectsSearchViewModel<T> : ObservableObject, IDomainObjectSearchViewModel where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectSearch<T> _domainObjectSearch;

      private IDomainObjectViewModelFactory _viewModelFactory;
      private ObservableCollection<DomainObjectViewModel> _searchResultViewModels;
      private MultiTextFilterViewModel _multiTextFilterViewModel;
      private PageNavigationViewModel _pageNavigationViewModel;
      private ObservableCollection<ConstraintViewModel<T>> _constraintViewModels;

      private bool _isExecutingSearch;

      #endregion

      #region Properties

      public ObservableCollection<DomainObjectViewModel> SearchResultViewModels
      {
         get => _searchResultViewModels;
         set => SetProperty(ref _searchResultViewModels, value);
      }

      public MultiTextFilterViewModel MultiTextFilterViewModel
      {
         get => _multiTextFilterViewModel;
         set => SetProperty(ref _multiTextFilterViewModel, value);
      }

      public PageNavigationViewModel PageNavigationViewModel
      {
         get => _pageNavigationViewModel;
         set => SetProperty(ref _pageNavigationViewModel, value);
      }

      public ObservableCollection<ConstraintViewModel<T>> ConstraintViewModels
      {
         get => _constraintViewModels;
         set => SetProperty(ref _constraintViewModels, value);
      }

      public ConstraintViewModel<T> SelectedConstraintViewModel => ConstraintViewModels.FirstOrDefault(vm => vm.IsSelected);

      public bool IsAnyConstraintSelected => SelectedConstraintViewModel != null;


      public bool IsExecutingSearch
      {
         get => _isExecutingSearch;
         set => SetProperty(ref _isExecutingSearch, value);
      }

      #endregion

      #region Events

      public event EventHandler PreviewUnloadSearchResultViewModels;

      #endregion

      #region Commands

      public AsyncRelayCommand SearchCommandAsync { get; private set; }
      public AsyncRelayCommand<bool> PaginatedSearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectsSearchViewModel(DomainObjectSearch<T> domainObjectSearch, IDomainObjectViewModelFactory viewModelFactory)
      {
         _domainObjectSearch = domainObjectSearch;
         _viewModelFactory = viewModelFactory;

         SearchResultViewModels = new ObservableCollection<DomainObjectViewModel>();

         SearchCommandAsync = new AsyncRelayCommand(SearchAsync);
         PaginatedSearchCommandAsync = new AsyncRelayCommand<bool>(PaginatedSearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);

         ConstraintViewModels = new ObservableCollection<ConstraintViewModel<T>>();
         ConstructConstraintViewModels();

         MultiTextFilterViewModel = new MultiTextFilterViewModel(_domainObjectSearch.MultiTextFilter);
         MultiTextFilterViewModel.SearchRequested += MultiTextFilterViewModel_SearchRequested;

         PageNavigationViewModel = new PageNavigationViewModel(domainObjectSearch.PageNavigation);
         PageNavigationViewModel.SelectPageRequested += PageNavigationViewModel_SelectPageRequested;
      }

      #endregion

      #region Public Methods

      public async Task SearchAsync(CancellationToken cancellationToken = default)
      {
         IsExecutingSearch = true;

         UnloadSearchResultViewModels();

         await _domainObjectSearch.SearchAsync(cancellationToken);

         LoadSearchResultViewModels();

         IsExecutingSearch = false;
      }

      public async Task PaginatedSearchAsync(bool resetPage, CancellationToken cancellationToken = default)
      {
         IsExecutingSearch = true;

         UnloadSearchResultViewModels();

         await _domainObjectSearch.PaginatedSearchAsync(resetPage, cancellationToken);

         LoadSearchResultViewModels();

         if (resetPage)
         {
           PageNavigationViewModel.Refresh();
         }

         IsExecutingSearch = false;
      }

      public void LoadSearchResultViewModels()
      {
         foreach (var searchResult in _domainObjectSearch.SearchResults)
         {
            var viewModel = _viewModelFactory.CreateViewModel(searchResult);
            SearchResultViewModels.Add(viewModel);
         }
      }

      public void UnloadSearchResultViewModels()
      {
         PreviewUnloadSearchResultViewModels?.Invoke(this, new EventArgs());

         foreach (var viewModel in SearchResultViewModels)
         {
            viewModel.Dispose();
         }

         SearchResultViewModels.Clear();
      }

      public void AddSearchResultViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         SearchResultViewModels.Add(domainObjectViewModel);
      }

      public void RemoveSearchResultViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         domainObjectViewModel.Dispose();
         SearchResultViewModels.Remove(domainObjectViewModel);
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         MultiTextFilterViewModel.SearchRequested -= MultiTextFilterViewModel_SearchRequested;

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);

         PageNavigationViewModel.PropertyChanged -= PageNavigationViewModel_SelectPageRequested;
      }

      #endregion

      #region Private Methods  

      private void ConstructConstraintViewModels()
      {
         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);
         ConstraintViewModels.Clear();

         foreach (var constraint in _domainObjectSearch.GetConstraints())
         {
            ConstraintViewModels.Add(new ConstraintViewModel<T>(constraint));
         }

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged += ConstraintViewModel_PropertyChanged);
      }

      private async void MultiTextFilterViewModel_SearchRequested(object? sender, EventArgs e)
      {
         await PaginatedSearchAsync(true, new CancellationToken());
      }

      private async void ConstraintViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(ConstraintViewModel<T>.IsSelected)))
         {
            if ((sender as ConstraintViewModel<T>).IsSelected)
            {
               ConstraintViewModels.Where(vm => vm != sender).ToList().ForEach(vm => vm.IsSelected = false);
               _domainObjectSearch.ActivateConstraint(SelectedConstraintViewModel?.Constraint);
               await PaginatedSearchAsync(true, new CancellationToken());
            }
            else if (ConstraintViewModels.All(vm => !vm.IsSelected))
            {
               _domainObjectSearch.ActivateConstraint(null);
               await PaginatedSearchAsync(true, new CancellationToken());
            }
         }
      }

      private async void PageNavigationViewModel_SelectPageRequested(object? sender, EventArgs e)
      {
         await PaginatedSearchAsync(false, new CancellationToken());
      }      

      private void CancelSearch()
      {
         SearchCommandAsync.Cancel();
      }      

      #endregion
   }
}
