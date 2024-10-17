using OtherSideCore.Adapter;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public class DomainObjectSearch<T> : IDomainObjectSearch<T> where T : DomainObject, new()
   {
      #region Fields

      private readonly int _pageSize = 20;

      protected IDomainObjectQueryService<T> _domainObjectQueryService;
      private List<DomainObject> _searchResults;
      private MultiTextFilter _multiTextFilter;
      private PageNavigation _pageNavigation;

      private Func<CancellationToken, Task> _selectedSearchResultChangedAsync;

      private Constraint<T> _activatedConstraint;

      private List<Constraint<T>> _constraints;

      #endregion

      #region Properties

      public PageNavigation PageNavigation => _pageNavigation;
      public List<DomainObject> SearchResults => _searchResults;
      public MultiTextFilter MultiTextFilter => _multiTextFilter;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSearch(IDomainObjectQueryService<T> domainObjectQueryService)
      {
         _domainObjectQueryService = domainObjectQueryService;
         _searchResults = new List<DomainObject>();
         _multiTextFilter = new MultiTextFilter();
         _constraints = new List<Constraint<T>>();
         _pageNavigation = new PageNavigation();
      }

      #endregion

      #region Public Methods

      public void ClearFilters()
      {
         _multiTextFilter.Filters.Clear();
      }

      public void ClearConstraints()
      {
         _constraints.Clear();
      }

      public List<Constraint<T>> GetConstraints()
      {
         return _constraints;
      }

      public void SetConstraints(List<Constraint<T>> constraints)
      {
         _constraints.Clear();
         _constraints.AddRange(constraints);
      }

      public async Task SearchAsync(CancellationToken cancellationToken)
      {
         try
         {
            UnloadSearchResults();

            var results = await _domainObjectQueryService.SearchAsync(_multiTextFilter.StringFilters,
                                                                      _activatedConstraint,
                                                                      _multiTextFilter.ExtendedSearch,
                                                                      cancellationToken);

            _searchResults.AddRange(results);
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResults();
         }
      }

      public async Task PaginatedSearchAsync(bool resetPages, CancellationToken cancellationToken)
      {
         try
         {
            UnloadSearchResults();

            var pageToSelect = resetPages ? 1 : Math.Max(1, _pageNavigation.CurrentPageNumber);

            var pagedResult = await _domainObjectQueryService.PaginatedSearchAsync(_multiTextFilter.StringFilters,
                                                                                   _activatedConstraint,
                                                                                   _multiTextFilter.ExtendedSearch,
                                                                                   pageToSelect,
                                                                                   _pageSize,
                                                                                   cancellationToken);

            _searchResults.AddRange(pagedResult.Items);

            if (resetPages)
            {
               _pageNavigation.SetPages(pagedResult.TotalPages, pagedResult.PageSize, pagedResult.TotalCount);
               _pageNavigation.SelectFirstPage();
            }
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResults();
         }
      }

      public void AddSearchResult(DomainObject domainObject)
      {
         _searchResults.Add(domainObject);
      }

      public void RemoveSearchResult(DomainObject domainObject)
      {
         _searchResults.Remove(domainObject);
      }

      public void ActivateConstraint(Constraint<T> constraint)
      {
         _activatedConstraint = constraint;
      }

      public void Dispose()
      {
         UnloadSearchResults();
      }

      #endregion

      #region Private Methods

      private void UnloadSearchResults()
      {
         _searchResults.ForEach(r => r.Dispose());
         _searchResults.Clear();
      }

      #endregion
   }
}
