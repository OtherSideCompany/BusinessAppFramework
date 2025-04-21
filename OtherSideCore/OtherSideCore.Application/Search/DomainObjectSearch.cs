using Microsoft.Data.SqlClient;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Search
{
   public class DomainObjectSearch<T> : IDomainObjectSearch<T> where T : DomainObject, new()
   {
      #region Fields

      protected IDomainObjectQueryService<T> _domainObjectQueryService;
      protected List<DomainObjectSearchResult> _searchResults;
      protected PageNavigation _pageNavigation;

      private Func<CancellationToken, Task> _selectedSearchResultChangedAsync;
      protected Constraint<T> _activatedConstraint;
      private List<Constraint<T>> _activableConstraints;
      private List<Constraint<T>> _filterConstraints;

      protected CancellationTokenSource? _currentSearchCancellationTokenSource;
      private readonly SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1, 1);
      protected CancellationToken _currentSearchCancellationToken => _currentSearchCancellationTokenSource.Token;

      #endregion

      #region Properties

      public int PageSize = 20;
      public PageNavigation PageNavigation => _pageNavigation;
      public List<DomainObjectSearchResult> SearchResults => _searchResults;
      public Constraint<T> ActivatedConstraint => _activatedConstraint;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSearch(
         IDomainObjectQueryService<T> domainObjectQueryService)
      {
         _domainObjectQueryService = domainObjectQueryService;
         _searchResults = new List<DomainObjectSearchResult>();
         _activableConstraints = new List<Constraint<T>>();
         _filterConstraints = new List<Constraint<T>>();
         _pageNavigation = new PageNavigation();
      }

      #endregion

      #region Public Methods

      public void ClearActivableConstraints()
      {
         _activableConstraints.Clear();
      }

      public void ClearFilterConstraints()
      {
         _filterConstraints.Clear();
      }

      public List<Constraint<T>> GetActivableConstraints()
      {
         return _activableConstraints;
      }

      public void SetActivableConstraints(List<Constraint<T>> constraints)
      {
         ClearActivableConstraints();
         _activableConstraints.AddRange(constraints);
      }

      public void SetFilterConstraints(List<Constraint<T>> constraints)
      {
         ClearFilterConstraints();
         _filterConstraints.AddRange(constraints);
      }

      public async Task SearchAsync(bool extendedSearch, List<string> filters, DomainObject parent = null)
      {
         await InitializeSearchAsync();

         try
         {
            await SearchAsync(extendedSearch, filters, parent, _currentSearchCancellationTokenSource.Token);
         }
         catch (InvalidOperationException)
         {
            UnloadSearchResults();
         }
         catch (SqlException)
         {
            UnloadSearchResults();
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResults();
         }
         finally
         {
            ShutdownSearch();
         }
      }

      public async Task<DomainObjectSearchResult> GetSearchResultAsync(int domainObjectId)
      {
         await InitializeSearchAsync();

         DomainObjectSearchResult searchResult = null;

         try
         {
            searchResult = await SearchAsync(domainObjectId, _currentSearchCancellationTokenSource.Token);
         }
         catch (InvalidOperationException)
         {
            UnloadSearchResults();
         }
         catch (SqlException)
         {
            UnloadSearchResults();
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResults();
         }
         finally
         {
            ShutdownSearch();
         }

         return searchResult;
      }

      public async Task PaginatedSearchAsync(bool resetPages, bool extendedSearch, List<string> filters, DomainObject parent = null)
      {
         await InitializeSearchAsync();

         try
         {
            await PaginatedSearchAsync(resetPages, extendedSearch, filters, parent, _currentSearchCancellationTokenSource.Token);
         }
         catch (InvalidOperationException e)
         {
            UnloadSearchResults();
         }
         catch (SqlException)
         {
            UnloadSearchResults();
         }
         catch (OperationCanceledException)
         {
            UnloadSearchResults();
         }
         finally
         {
            ShutdownSearch();
         }
      }

      public async Task AddSearchResultAsync(int domainObjectId)
      {
         var searchResult = await _domainObjectQueryService.SearchAsync(domainObjectId);
         _searchResults.Add(searchResult);
      }

      public void RemoveSearchResult(int domainObjectId)
      {
         _searchResults.RemoveAll(sr => sr.DomainObjectId == domainObjectId);
      }

      public void ActivateConstraint(Constraint<T> constraint)
      {
         _activatedConstraint = constraint;
      }

      public void CancelSearch()
      {
         ShutdownSearch();
         UnloadSearchResults();
      }

      public void Dispose()
      {
         _currentSearchCancellationTokenSource?.Cancel();
         DisposeSearchCancellationTokenSource();

         UnloadSearchResults();
      }

      #endregion

      #region Private Methods

      protected async Task InitializeSearchAsync()
      {
         await _searchSemaphore.WaitAsync();
         CreateSearchCancellationTokenSource();

         UnloadSearchResults();
      }

      protected void ShutdownSearch()
      {
         DisposeSearchCancellationTokenSource();

         if (_searchSemaphore.CurrentCount == 0)
         {
            _searchSemaphore.Release();
         }
      }

      private Constraint<T> GetConstraints()
      {
         var expression = _activatedConstraint != null ? _activatedConstraint.Expression : x => true;

         foreach (var filterConstraint in _filterConstraints)
         {
            expression = expression.And(filterConstraint.Expression);
         }

         return new Constraint<T>(expression);
      }

      protected async Task SearchAsync(bool extendedSearch, List<string> filters, DomainObject? parent, CancellationToken cancellationToken)
      {
         var results = await _domainObjectQueryService.SearchAsync(filters,
                                                                   GetConstraints(),
                                                                   parent,
                                                                   extendedSearch,
                                                                   cancellationToken);

         if (results != null)
         {
            _searchResults.AddRange(results);
         }
      }

      protected async Task<DomainObjectSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken)
      {
         return await _domainObjectQueryService.SearchAsync(domainObjectId, cancellationToken);
      }

      protected async Task PaginatedSearchAsync(bool resetPages, bool extendedSearch, List<string> filters, DomainObject? parent, CancellationToken cancellationToken)
      {
         var pageToSelect = resetPages ? 1 : Math.Max(1, _pageNavigation.CurrentPageNumber);

         var pagedResult = await _domainObjectQueryService.PaginatedSearchAsync(filters,
                                                                                GetConstraints(),
                                                                                parent,
                                                                                extendedSearch,
                                                                                pageToSelect,
                                                                                PageSize,
                                                                                cancellationToken);

         _searchResults.AddRange(pagedResult.Items);

         if (resetPages)
         {
            _pageNavigation.SetPages(pagedResult.TotalPages, pagedResult.PageSize, pagedResult.TotalCount);
            _pageNavigation.SelectFirstPage();
         }
      }

      protected void CreateSearchCancellationTokenSource()
      {
         if (_currentSearchCancellationTokenSource != null)
         {
            _currentSearchCancellationTokenSource.Cancel();
            _currentSearchCancellationTokenSource.Dispose();
            _currentSearchCancellationTokenSource = null;
         }

         _currentSearchCancellationTokenSource = new CancellationTokenSource();
         _currentSearchCancellationTokenSource.Token.ThrowIfCancellationRequested();
      }

      protected void DisposeSearchCancellationTokenSource()
      {
         _currentSearchCancellationTokenSource?.Dispose();
         _currentSearchCancellationTokenSource = null;
      }

      protected void UnloadSearchResults()
      {
         _searchResults.ForEach(r => r.Dispose());
         _searchResults.Clear();
      }

      #endregion
   }
}
