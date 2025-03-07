using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
    public class DomainObjectsSearchViewModel<T> : ObservableObject, IDomainObjectSearchViewModel where T : DomainObject, new()
   {
      #region Fields

      private bool _isInAdvancedSearchMode;

      protected DomainObjectSearch<T> _domainObjectSearch;

      private IDomainObjectSearchResultViewModelFactory _domainObjectSearchResultViewModelFactory;
      private IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;
      private ObservableCollection<DomainObjectSearchResultViewModel> _searchResultViewModels;
      private SingleTextFilterViewModel _singleTextFilterViewModel;
      private MultiTextFilterViewModel _multiTextFilterViewModel;
      private PageNavigationViewModel _pageNavigationViewModel;
      private ObservableCollection<ConstraintViewModel<T>> _constraintViewModels;

      private bool _isExecutingSearch;

      #endregion

      #region Properties

      public bool IsInAdvancedSearchMode
      {
         get => _isInAdvancedSearchMode;
         set => SetProperty(ref _isInAdvancedSearchMode, value);
      }

      public ObservableCollection<DomainObjectSearchResultViewModel> SearchResultViewModels
      {
         get => _searchResultViewModels;
         set => SetProperty(ref _searchResultViewModels, value);
      }

      public SingleTextFilterViewModel SingleTextFilterViewModel
      {
         get => _singleTextFilterViewModel;
         set => SetProperty(ref _singleTextFilterViewModel, value);
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

      public AsyncRelayCommand<SearchParameters> SearchCommandAsync { get; private set; }
      public AsyncRelayCommand<PaginatedSearchParameters> PaginatedSearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectsSearchViewModel(
         DomainObjectSearch<T> domainObjectSearch, 
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory)
      {
         _domainObjectSearch = domainObjectSearch;
         _domainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;

         SearchResultViewModels = new ObservableCollection<DomainObjectSearchResultViewModel>();

         SearchCommandAsync = new AsyncRelayCommand<SearchParameters>(SearchAsync);
         PaginatedSearchCommandAsync = new AsyncRelayCommand<PaginatedSearchParameters>(PaginatedSearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);

         ConstraintViewModels = new ObservableCollection<ConstraintViewModel<T>>();
         ConstructConstraintViewModels();

         MultiTextFilterViewModel = new MultiTextFilterViewModel();
         SingleTextFilterViewModel = new SingleTextFilterViewModel();

         PageNavigationViewModel = new PageNavigationViewModel(domainObjectSearch.PageNavigation);
         PageNavigationViewModel.SelectPageRequested += PageNavigationViewModel_SelectPageRequested;
      }

      #endregion

      #region Public Methods

      public async Task SearchAsync(SearchParameters parameters)
      {
         await SearchAsync(parameters.ExtendedSearch, parameters.ParentViewModel);
      }

      public async Task PaginatedSearchAsync(PaginatedSearchParameters parameters)
      {
         await PaginatedSearchAsync(parameters.ResetPage, parameters.ExtendedSearch, parameters.ParentViewModel);
      }

      public virtual void LoadSearchResultViewModels()
      {
         var snapshot = _domainObjectSearch.SearchResults.ToList();

         foreach (var searchResult in snapshot)
         {
            var viewModel = _domainObjectSearchResultViewModelFactory.CreateViewModel(searchResult);
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

      public void AddSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         SearchResultViewModels.Add(domainObjectSearchResultViewModel);
      }

      public async Task<DomainObjectSearchResultViewModel> AddSearchResultViewModelAsync(int domainObjectId)
      {
         var searchResult = await _domainObjectQueryServiceFactory.CreateDomainObjectQueryService<T>().SearchAsync(domainObjectId);
         var searchResultViewModel = _domainObjectSearchResultViewModelFactory.CreateViewModel(searchResult);
         SearchResultViewModels.Add(searchResultViewModel);

         return searchResultViewModel;
      }
      public async Task<DomainObjectSearchResultViewModel> InsertSearchResultViewModelAsync(int domainObjectId, int index)
      {
         var searchResult = await _domainObjectQueryServiceFactory.CreateDomainObjectQueryService<T>().SearchAsync(domainObjectId);
         var searchResultViewModel = _domainObjectSearchResultViewModelFactory.CreateViewModel(searchResult);
         SearchResultViewModels.Insert(index, searchResultViewModel);

         return searchResultViewModel;
      }

      public void RemoveSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         domainObjectSearchResultViewModel.Dispose();
         SearchResultViewModels.Remove(domainObjectSearchResultViewModel);
      }

      public void RemoveSearchResultViewModel(int domainObjectId)
      {
         var domainObjectSearchResultViewModel = SearchResultViewModels.FirstOrDefault(vm => vm.DomainObjectSearchResult.DomainObjectId == domainObjectId);
         RemoveSearchResultViewModel(domainObjectSearchResultViewModel);
      }

      public async Task ReloadSearchResultAsync(int domainObjectId)
      {
         var searchResultViewModel = SearchResultViewModels.FirstOrDefault(vm => vm.DomainObjectSearchResult.DomainObjectId == domainObjectId);

         if (searchResultViewModel != null)
         {
            var searchResult = await _domainObjectSearch.GetSearchResultAsync(domainObjectId);

            if (searchResult != null)
            {
               searchResultViewModel.UpdateDomainObjectSearchResult(searchResult);
            }
         }
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);

         PageNavigationViewModel.PropertyChanged -= PageNavigationViewModel_SelectPageRequested;
      }

      #endregion

      #region Private Methods  

      protected virtual async Task SearchAsync(bool extendedSearch, DomainObjectViewModel? parentViewModel)
      {
         IsExecutingSearch = true;

         UnloadSearchResultViewModels();

         await _domainObjectSearch.SearchAsync(extendedSearch, GetTextFilters(), parentViewModel?.DomainObject);

         LoadSearchResultViewModels();

         IsExecutingSearch = false;
      }

      protected virtual async Task PaginatedSearchAsync(bool resetPage, bool extendedSearch, DomainObjectViewModel? parentViewModel)
      {
         IsExecutingSearch = true;

         UnloadSearchResultViewModels();

         await _domainObjectSearch.PaginatedSearchAsync(resetPage, extendedSearch, GetTextFilters(), parentViewModel?.DomainObject);

         LoadSearchResultViewModels();

         if (resetPage)
         {
            PageNavigationViewModel.Refresh();
         }

         IsExecutingSearch = false;
      }

      protected List<string> GetTextFilters()
      {
         if (IsInAdvancedSearchMode)
         {
            return _multiTextFilterViewModel.Filters.Where(f => !String.IsNullOrEmpty(f.Text)).Select(f => f.Text).ToList();
         }
         else
         {
            return String.IsNullOrEmpty(_singleTextFilterViewModel.Filter) ? [] : new List<string>() { _singleTextFilterViewModel.Filter };
         }
      }

      protected void ConstructConstraintViewModels()
      {
         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);
         ConstraintViewModels.Clear();

         foreach (var constraint in _domainObjectSearch.GetConstraints())
         {
            var constraintViewModel = new ConstraintViewModel<T>(constraint);
            constraintViewModel.IsSelected = _domainObjectSearch.ActivatedConstraint == null ? false : _domainObjectSearch.ActivatedConstraint.Equals(constraintViewModel.Constraint);
            ConstraintViewModels.Add(constraintViewModel);
         }

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged += ConstraintViewModel_PropertyChanged);
      }

      private async void ConstraintViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(ConstraintViewModel<T>.IsSelected)))
         {
            if ((sender as ConstraintViewModel<T>).IsSelected)
            {
               ConstraintViewModels.Where(vm => vm != sender).ToList().ForEach(vm => vm.IsSelected = false);
               _domainObjectSearch.ActivateConstraint(SelectedConstraintViewModel?.Constraint);
               await PaginatedSearchAsync(new PaginatedSearchParameters() { ResetPage = true });
            }
            else if (ConstraintViewModels.All(vm => !vm.IsSelected))
            {
               _domainObjectSearch.ActivateConstraint(null);
               await PaginatedSearchAsync(new PaginatedSearchParameters() { ResetPage = true });
            }
         }
      }

      private async void PageNavigationViewModel_SelectPageRequested(object? sender, EventArgs e)
      {
         await PaginatedSearchAsync(new PaginatedSearchParameters());
      }

      private void CancelSearch()
      {
         _domainObjectSearch.CancelSearch();
      }      

      #endregion
   }
}
