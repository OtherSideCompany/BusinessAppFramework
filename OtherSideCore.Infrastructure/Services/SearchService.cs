using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Application.Trees;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Services
{
    public class SearchService<TSearchResult> : ISearchService<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        protected readonly IDbContextFactory<DbContext> _dbContextFactory;
        protected ILogger<SearchService<TSearchResult>> _logger;
        protected IConstraintFactory _constraintFactory;

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public SearchService(
           IDbContextFactory<DbContext> dbContextFactory,
           ILoggerFactory loggerFactory,
           IConstraintFactory constraintFactory)
        {
            _dbContextFactory = dbContextFactory;
            _logger = loggerFactory.CreateLogger<SearchService<TSearchResult>>();
            _constraintFactory = constraintFactory;
        }

        #endregion

        #region Public Methods

        public virtual async Task<SearchResult<TSearchResult>> SearchAsync(
           SearchRequest searchRequest,
           CancellationToken cancellationToken)
        {
            LogSearchAsync(nameof(SearchAsync), searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch);

            return new SearchResult<TSearchResult>
            {
                Items = await SearchAsync(searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch, false, 0, 0, cancellationToken),
                Count = await CountAsync(searchRequest, cancellationToken)
            };
        }

        public virtual async Task<SearchResult<TSearchResult>> PaginatedSearchAsync(
           PaginatedSearchRequest paginatedSearchRequest,
           CancellationToken cancellationToken)
        {
            LogSearchAsync(nameof(PaginatedSearchAsync), paginatedSearchRequest.ConstraintKey, paginatedSearchRequest.Filters, paginatedSearchRequest.ExtendedSearch);

            return new SearchResult<TSearchResult>
            {
                Items = await SearchAsync(paginatedSearchRequest.ConstraintKey, paginatedSearchRequest.Filters, paginatedSearchRequest.ExtendedSearch, true, paginatedSearchRequest.PageIndex, paginatedSearchRequest.PageSize, cancellationToken),
                Count = await CountAsync(paginatedSearchRequest, cancellationToken)
            };
        }


        public virtual async Task<int> CountAsync(SearchRequest searchRequest, CancellationToken cancellationToken)
        {
            LogSearchAsync(nameof(CountAsync), searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch);

            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await GetSearchQuery(searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch, false, 0, 0, context).CountAsync(cancellationToken);
            }
        }

        public async Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Type}, {MethodName}, DomainObjectId = {DomainObjectId}", GetType(), nameof(SearchAsync), domainObjectId);

            using (var context = _dbContextFactory.CreateDbContext())
            {
                var query = context.Set<TSearchResult>().AsNoTracking();

                query = query.Where(e => e.DomainObjectId == domainObjectId);

                return await query.FirstAsync(cancellationToken);
            }
        }

        public async Task<NodeSummary> GetSummaryAsync(int domainObjectId)
        {
            var result = await SearchAsync(domainObjectId, CancellationToken.None);
            var nodeSummary = result.GetSummary();

            return nodeSummary;
        }

        #endregion

        #region Private Methods

        private async Task<List<TSearchResult>> SearchAsync(string constraintKey,
                                                            List<string> filters,
                                                            bool extendedSearch,
                                                            bool paginated,
                                                            int pageIndex,
                                                            int pageSize,
                                                            CancellationToken cancellationToken)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var result = await GetSearchQuery(constraintKey, filters, extendedSearch, paginated, pageIndex, pageSize, context).ToListAsync(cancellationToken);
                return result;
            }
        }

        protected IQueryable<TSearchResult> GetSearchQuery(string constraintKey, 
                                                           List<string> filters,
                                                           bool extendedSearch,
                                                           bool paginated,
                                                           int pageIndex,
                                                           int pageSize,
                                                           DbContext context)
        {
            var query = context.Set<TSearchResult>().AsNoTracking();            

            query = query.OrderByDescending(e => e.DomainObjectId);

            if (!constraintKey.Equals(Contracts.ConstraintKeys.AllConstraintKey))
            {
                var constraint = _constraintFactory.GetConstraint<TSearchResult>(StringKey.From(constraintKey));
                query = query.Where(constraint);
            }

            foreach (var filterConstraint in GetFilterConstraints(filters, extendedSearch))
            {
                query = query.Where(filterConstraint);
            }

            if (paginated)
            {
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return query;
        }


        protected List<Expression<Func<TSearchResult, bool>>> GetFilterConstraints(List<string> filters, bool extendedSearch)
        {
            var constraints = new List<Expression<Func<TSearchResult, bool>>>();

            foreach (var filter in filters)
            {
                var lowerFilter = filter.ToLower();
                var maxSearchDistance = Utils.GetMaxSearchDistance(lowerFilter);

                constraints.Add(GetFilterConstraint(lowerFilter, extendedSearch, maxSearchDistance));
            }

            return constraints;
        }

        protected virtual Expression<Func<TSearchResult, bool>> GetFilterConstraint(string lowerFilter, bool extendedSearch, int maxSearchDistance)
        {
            return x => false;
        }

        private void LogSearchAsync(string methodName, string constraint, List<string> filters, bool extendedSearch)
        {
            var filterString = (filters == null || !filters.Any()) ? "none" : string.Join(',', filters);
            var constraintString = String.IsNullOrEmpty(constraint) ? "none" : constraint;

            var msg = $"{GetType()}, {methodName}, constraint : {constraintString}, filters : {filterString}, extendedSearch : {extendedSearch}";
            _logger.LogInformation(msg);
        }

        #endregion
    }
}
