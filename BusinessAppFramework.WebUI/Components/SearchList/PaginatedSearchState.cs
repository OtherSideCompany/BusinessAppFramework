using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Components.SearchList
{
    public class PaginatedSearchState<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        private readonly ISearchGateway<TSearchResult> _searchGateway;
        private readonly IReadOnlyList<string> _constraintKeys;
        private readonly Dictionary<string, int> _constraintCounts = new();

        private int? _lastLoadedPage;

        public PaginatedSearchState(ISearchGateway<TSearchResult> searchGateway, IReadOnlyList<string> constraintKeys, string? defaultConstraintKey)
        {
            _searchGateway = searchGateway;
            _constraintKeys = constraintKeys;
            ConstraintKey = defaultConstraintKey ?? string.Empty;
        }

        public int PageSize { get; init; } = 20;
        public int CurrentPage { get; set; }
        public string ConstraintKey { get; set; }
        public int TotalItems { get; private set; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public int StartIndex => TotalPages == 0 ? 0 : (CurrentPage * PageSize) + 1;
        public int EndIndex => Math.Min((CurrentPage + 1) * PageSize, TotalItems);

        public int? GetConstraintCount(string constraintKey) =>
            _constraintCounts.TryGetValue(constraintKey, out var count) ? count : null;

        public async Task<GridData<TSearchResult>> LoadAsync(string? searchText, bool extendedSearch = false)
        {
            var request = new PaginatedSearchRequest
            {
                PageIndex = CurrentPage,
                PageSize = PageSize,
                ConstraintKey = ConstraintKey,
                ExtendedSearch = extendedSearch
            };

            if (!string.IsNullOrEmpty(searchText))
                request.Filters.Add(searchText);

            var result = await _searchGateway.PaginatedSearchAsync(request);

            var items = result?.Items?.ToList() ?? new List<TSearchResult>();
            TotalItems = result?.Count ?? 0;

            var isPageChangeOnly = _lastLoadedPage.HasValue && _lastLoadedPage.Value != CurrentPage;

            if (!isPageChangeOnly)
            {
                await RefreshConstraintCountsAsync();
            }

            _lastLoadedPage = CurrentPage;

            return new GridData<TSearchResult>
            {
                Items = items,
                TotalItems = TotalItems
            };
        }

        private async Task RefreshConstraintCountsAsync()
        {
            _constraintCounts.Clear();

            foreach (var constraintKey in _constraintKeys)
            {
                _constraintCounts[constraintKey] = await _searchGateway.CountAsync(new SearchRequest() { ConstraintKey = constraintKey });
            }
        }
    }
}
