using BusinessAppFramework.Application;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.Services
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
            LogSearchAsync(nameof(SearchAsync), searchRequest.ParentId, searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch);

            return new SearchResult<TSearchResult>
            {
                Items = await SearchAsync(searchRequest.ParentId, searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch, false, 0, 0, cancellationToken),
                Count = await CountAsync(searchRequest, cancellationToken)
            };
        }

        public virtual async Task<SearchResult<TSearchResult>> PaginatedSearchAsync(
           PaginatedSearchRequest paginatedSearchRequest,
           CancellationToken cancellationToken)
        {
            LogSearchAsync(nameof(PaginatedSearchAsync), paginatedSearchRequest.ParentId, paginatedSearchRequest.ConstraintKey, paginatedSearchRequest.Filters, paginatedSearchRequest.ExtendedSearch);

            return new SearchResult<TSearchResult>
            {
                Items = await SearchAsync(paginatedSearchRequest.ParentId, paginatedSearchRequest.ConstraintKey, paginatedSearchRequest.Filters, paginatedSearchRequest.ExtendedSearch, true, paginatedSearchRequest.PageIndex, paginatedSearchRequest.PageSize, cancellationToken),
                Count = await CountAsync(paginatedSearchRequest, cancellationToken)
            };
        }


        public virtual async Task<int> CountAsync(SearchRequest searchRequest, CancellationToken cancellationToken)
        {
            LogSearchAsync(nameof(CountAsync), searchRequest.ParentId, searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch);

            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await GetSearchQuery(searchRequest.ParentId, searchRequest.ConstraintKey, searchRequest.Filters, searchRequest.ExtendedSearch, context).CountAsync(cancellationToken);
            }
        }

        public async Task<TSearchResult> SearchAsync(int domainObjectId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Type}, {MethodName}, DomainObjectId = {DomainObjectId}", GetType(), nameof(SearchAsync), domainObjectId);

            using (var context = _dbContextFactory.CreateDbContext())
            {
                var query = GetBaseQuery(context).AsNoTracking();

                query = query.Where(e => e.DomainObjectId == domainObjectId);

                return await query.FirstAsync(cancellationToken);
            }
        }

        public async Task<List<TSearchResult>> SearchAllAsync(List<int> domainObjectIds, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Type}, {MethodName}, DomainObjectIds = {DomainObjectIds}", GetType(), nameof(SearchAllAsync), string.Join(", ", domainObjectIds));

            using var context = _dbContextFactory.CreateDbContext();

            var query = GetBaseQuery(context).AsNoTracking();

            query = query.Where(e => domainObjectIds.Contains(e.DomainObjectId));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<NodeSummary> GetSummaryAsync(int domainObjectId)
        {
            var result = await SearchAsync(domainObjectId, CancellationToken.None);
            var nodeSummary = result.GetSummary();

            return nodeSummary;
        }

        #endregion

        #region Private Methods

        private async Task<List<TSearchResult>> SearchAsync(int? parentId,
                                                            string constraintKey,
                                                            List<string> filters,
                                                            bool extendedSearch,
                                                            bool paginated,
                                                            int pageIndex,
                                                            int pageSize,
                                                            CancellationToken cancellationToken)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var query = GetSearchQuery(parentId, constraintKey, filters, extendedSearch, context);

                if (paginated)
                {
                    query = ApplyPagination(query, pageIndex, pageSize);
                }

                return await query.ToListAsync(cancellationToken);
            }
        }

        protected virtual IQueryable<TSearchResult> GetBaseQuery(DbContext context)
        {
            return context.Set<TSearchResult>();
        }

        protected IQueryable<TSearchResult> GetSearchQuery(int? parentId, 
                                                           string constraintKey,
                                                           List<string> filters,
                                                           bool extendedSearch,
                                                           DbContext context)
        {
            var query = GetBaseQuery(context).AsNoTracking();            

            query = query.OrderByDescending(e => e.DomainObjectId);

            if (!constraintKey.Equals(ConstraintKeys.AllConstraintKey))
            {
                var constraint = _constraintFactory.GetConstraint<TSearchResult>(constraintKey);
                query = query.Where(constraint);
            }

            foreach (var filterConstraint in GetFilterConstraints(filters, extendedSearch))
            {
                query = query.Where(filterConstraint);
            }

            return query;
        }

        protected IQueryable<TSearchResult> ApplyPagination(IQueryable<TSearchResult> query, int pageIndex, int pageSize)
        {
            return query.Skip(pageIndex * pageSize).Take(pageSize);
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

        private static readonly (string Name, bool IsNullable)[] _searchableProperties = BuildSearchableProperties();

        private static (string Name, bool IsNullable)[] BuildSearchableProperties()
        {
            var nullability = new NullabilityInfoContext();

            return typeof(TSearchResult)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string)
                            && p.CanRead
                            && p.GetCustomAttribute<NotSearchableAttribute>() is null)
                .Select(p => (p.Name, nullability.Create(p).ReadState != NullabilityState.NotNull))
                .ToArray();
        }

        protected virtual Expression<Func<TSearchResult, bool>> GetFilterConstraint(string lowerFilter, bool extendedSearch, int maxSearchDistance)
        {
            Expression<Func<TSearchResult, bool>> predicate = null;

            foreach (var (name, isNullable) in _searchableProperties)
            {
                var condition = BuildPropertyFilter(name, isNullable, lowerFilter, extendedSearch, maxSearchDistance);
                predicate = predicate == null ? condition : predicate.Or(condition);
            }

            return predicate ?? (x => false);
        }

        private static Expression<Func<TSearchResult, bool>> BuildPropertyFilter(
            string name, bool isNullable, string lowerFilter, bool extendedSearch, int maxSearchDistance)
        {
            if (extendedSearch)
            {
                if (isNullable)
                {
                    return x => EF.Property<string>(x, name) != null
                                && Utils.EditDistance(lowerFilter, EF.Property<string>(x, name).ToLower(), maxSearchDistance) <= maxSearchDistance;
                }

                return x => Utils.EditDistance(lowerFilter, EF.Property<string>(x, name).ToLower(), maxSearchDistance) <= maxSearchDistance;
            }

            if (isNullable)
            {
                return x => EF.Property<string>(x, name) != null
                            && EF.Property<string>(x, name).ToLower().Contains(lowerFilter);
            }

            return x => EF.Property<string>(x, name).ToLower().Contains(lowerFilter);
        }

        protected void LogSearchAsync(string methodName, int? parentId, string constraint, List<string> filters, bool extendedSearch)
        {
            var filterString = filters == null || !filters.Any() ? "none" : string.Join(',', filters);
            var constraintString = string.IsNullOrEmpty(constraint) ? "none" : constraint;
            var parentIdString = parentId.HasValue ? parentId.Value.ToString() : "none";

            var msg = $"{GetType()}, {methodName}, parentId : {parentIdString}, constraint : {constraintString}, filters : {filterString}, extendedSearch : {extendedSearch}";
            _logger.LogInformation(msg);
        }

        #endregion
    }
}
