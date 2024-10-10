using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter
{
   public class DomainObjectsSearchViewModel<T> : ObservableObject, IDomainObjectSearchViewModel where T : DomainObject, new()
   {
      #region Fields

      private readonly int _pageSize = 20;

      private IDomainObjectQueryService<T> _domainObjectQueryService;
      private IDomainObjectViewModelFactory _viewModelFactory;
      private ObservableCollection<DomainObjectViewModel> _searchResultViewModels;
      private MultiTextFilterViewModel _multiTextFilterViewModel;
      private PageNavigationViewModel _pageNavigationViewModel;
      private ObservableCollection<ConstraintViewModel<T>> _constraintViewModels;

      private bool _isSelectionLocked;
      private bool _isExecutingSearch;

      private Func<CancellationToken, Task> _selectedSearchResultChangedAsync;

      #endregion

      #region Properties

      public DomainObjectViewModel SelectedSearchResultViewModel
      {
         get
         {
            return SearchResultViewModels.FirstOrDefault(vm => vm.IsSelected);
         }
      }

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

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      public bool IsExecutingSearch
      {
         get => _isExecutingSearch;
         private set => SetProperty(ref _isExecutingSearch, value);
      }

      public Func<CancellationToken, Task> SelectedSearchResultChangedAsync
      {
         get => _selectedSearchResultChangedAsync;
         set => SetProperty(ref _selectedSearchResultChangedAsync, value);
      }

      #endregion

      #region Commands

      public AsyncRelayCommand SearchCommandAsync { get; private set; }
      public AsyncRelayCommand PaginatedSearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }
      public AsyncRelayCommand<DomainObjectViewModel> SelectSearchResultCommandAsync { get; private set; }
      public RelayCommand CancelSelectSearchResultCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectsSearchViewModel(IUserContext userContext, IDomainObjectQueryService<T> domainObjectQueryService, IDomainObjectService<T> domainObjectService, IDomainObjectViewModelFactory viewModelFactory)
      {
         _domainObjectQueryService = domainObjectQueryService;
         SearchResultViewModels = new ObservableCollection<DomainObjectViewModel>();
         _viewModelFactory = viewModelFactory;

         PaginatedSearchCommandAsync = new AsyncRelayCommand(PaginatedSearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);
         SelectSearchResultCommandAsync = new AsyncRelayCommand<DomainObjectViewModel>(SelectSearchResultAsync, CanSelectSearchResult);
         CancelSelectSearchResultCommand = new RelayCommand(CancelSelectSearchResult);

         ConstraintViewModels = new ObservableCollection<ConstraintViewModel<T>>();

         MultiTextFilterViewModel = new MultiTextFilterViewModel(new MultiTextFilter(true));
         MultiTextFilterViewModel.SearchRequested += MultiTextFilterViewModel_SearchRequested;

         PageNavigationViewModel = new PageNavigationViewModel();
         PageNavigationViewModel.PropertyChanged += PageNavigationViewModel_PropertyChanged;
      }


      #endregion

      #region Public Methods

      public async Task SearchAsync(CancellationToken cancellationToken)
      {
         await SearchAsync(MultiTextFilterViewModel.MultiTextFilter.StringFilters, 
                           MultiTextFilterViewModel.ExtendedSearch, 
                           1,
                           SelectedConstraintViewModel?.Constraint,
                           true,
                           cancellationToken);
      }

      public async Task PaginatedSearchAsync(CancellationToken cancellationToken)
      {
         await SearchAsync(MultiTextFilterViewModel.MultiTextFilter.StringFilters, 
                           MultiTextFilterViewModel.ExtendedSearch, 
                           PageNavigationViewModel.CurrentPageNumber,
                           SelectedConstraintViewModel?.Constraint,
                           false,
                           cancellationToken);
      }     

      public bool CanSelectSearchResult(DomainObjectViewModel domainObjectViewModel)
      {
         return !IsSelectionLocked && domainObjectViewModel != null && !domainObjectViewModel.IsSelected;
      }

      public async Task SelectSearchResultAsync(DomainObjectViewModel domainObjectViewModel, CancellationToken cancellationToken)
      {
         try
         {
            UnselectSearchResult();

            if (domainObjectViewModel != null)
            {
               domainObjectViewModel.IsSelected = true;
               OnPropertyChanged(nameof(SelectedSearchResultViewModel));

               if (SelectedSearchResultChangedAsync != null)
               {
                  await SelectedSearchResultChangedAsync(cancellationToken);
               }
            }
         }
         catch (OperationCanceledException)
         {
            UnselectSearchResult();
         }
      }

      public void UnselectSearchResult()
      {
         if (SelectedSearchResultViewModel != null)
         {
            SelectedSearchResultViewModel.IsSelected = false;
            OnPropertyChanged(nameof(SelectedSearchResultViewModel));
         }
      }

      public void LockSelection()
      {
         IsSelectionLocked = true;
      }

      public void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      public void SetConstraintViewModels(List<ConstraintViewModel<T>> constraintViewModels)
      {
         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);
         ConstraintViewModels.Clear();

         foreach (var constraintViewModel in constraintViewModels)
         {
            ConstraintViewModels.Add(constraintViewModel);
         }

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged += ConstraintViewModel_PropertyChanged);
      }

      public void AddSearchResultViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         SearchResultViewModels.Add(domainObjectViewModel);
      }

      public void RemoveSearchResultViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         domainObjectViewModel.Dispose();

         if (SelectedSearchResultViewModel != null && SelectedSearchResultViewModel.Equals(domainObjectViewModel))
         {
            UnselectSearchResult();
         }

         SearchResultViewModels.Remove(domainObjectViewModel);
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();

         MultiTextFilterViewModel.SearchRequested -= MultiTextFilterViewModel_SearchRequested;
         MultiTextFilterViewModel.Dispose();

         ConstraintViewModels.ToList().ForEach(vm => vm.PropertyChanged -= ConstraintViewModel_PropertyChanged);

         PageNavigationViewModel.PropertyChanged -= PageNavigationViewModel_PropertyChanged;
      }

      #endregion

      #region Private Methods  

      private async Task SearchAsync(List<string> filters, bool extendedSearch, int pageNumber, Constraint<T> constraint, bool resetPages, CancellationToken cancellationToken)
      {
         IsExecutingSearch = true;

         try
         {
            var selectedDomainObject = SelectedSearchResultViewModel?.DomainObject;

            UnselectSearchResult();

            UnloadSearchResultViewModels();

            var pagedResult = await _domainObjectQueryService.PaginatedSearchAsync(filters,
                                                                                   constraint,
                                                                                   extendedSearch,
                                                                                   pageNumber,
                                                                                   _pageSize,
                                                                                   cancellationToken);

            ConstructSearchResultViewModels(pagedResult.Items);

            if (resetPages)
            {
               PageNavigationViewModel.SetPages(pagedResult.TotalPages, pagedResult.PageSize, pagedResult.TotalCount);
            }

            if (selectedDomainObject != null)
            {
               await SelectSearchResultAsync(SearchResultViewModels.FirstOrDefault(vm => vm.DomainObject.Equals(selectedDomainObject)), cancellationToken);
            }
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResultViewModels();
         }

         IsExecutingSearch = false;
      }

      private async void MultiTextFilterViewModel_SearchRequested(object? sender, EventArgs e)
      {
         await SearchAsync(new CancellationToken());
      }

      private async void ConstraintViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(ConstraintViewModel<T>.IsSelected)))
         {
            await SearchAsync(new CancellationToken());
         }
      }

      private async void PageNavigationViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(PageNavigationViewModel.CurrentPageNumber)))
         {
            await PaginatedSearchAsync(new CancellationToken());
         }
      }

      protected void ConstructSearchResultViewModels(IEnumerable<T> domainObjects)
      {
         foreach (var searchResult in domainObjects)
         {
            var viewModel = _viewModelFactory.CreateViewModel(searchResult);
            AddSearchResultViewModel(viewModel);
         }
      }

      private void CancelSearch()
      {
         SearchCommandAsync.Cancel();
      }      

      private void CancelSelectSearchResult()
      {
         SelectSearchResultCommandAsync.Cancel();
      }

      private void UnloadSearchResultViewModels()
      {
         foreach (var viewModel in SearchResultViewModels)
         {
            viewModel.Dispose();
         }

         SearchResultViewModels.Clear();
      }

      #endregion
   }
}
