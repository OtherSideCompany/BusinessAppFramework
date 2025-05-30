using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
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

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SearchService(
         IDbContextFactory<DbContext> dbContextFactory,
         ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _logger = loggerFactory.CreateLogger<SearchService<TSearchResult>>();
      }

      #endregion

      #region Public Methods

      public virtual async Task<List<TSearchResult>> SearchAsync(
         List<string> filters,
         bool extendedSearch,
         Expression<Func<TSearchResult, bool>> constraints,
         CancellationToken cancellationToken)
      {
         LogSearchAsync(nameof(SearchAsync), filters, extendedSearch);

         return await SearchAsync(filters, extendedSearch, constraints, false, 0, 0, cancellationToken);
      }

      public virtual async Task<List<TSearchResult>> PaginatedSearchAsync(
         List<string> filters,
         bool extendedSearch,
         Expression<Func<TSearchResult, bool>> constraints,
         int pageNumber,
         int pageSize,
         CancellationToken cancellationToken)
      {
         LogSearchAsync(nameof(PaginatedSearchAsync), filters, extendedSearch);

         return await SearchAsync(filters, extendedSearch, constraints, true, pageNumber, pageSize, cancellationToken);
      }


      public virtual async Task<int> CountAsync(List<string> filters, bool extendedSearch, Expression<Func<TSearchResult, bool>> predicate, CancellationToken cancellationToken)
      {
         LogSearchAsync(nameof(CountAsync), filters, extendedSearch);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await GetSearchQuery(filters, extendedSearch, predicate, false, 0, 0, context).CountAsync(cancellationToken);
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

      #endregion

      #region Private Methods

      private async Task<List<TSearchResult>> SearchAsync(List<string> filters,
                                                          bool extendedSearch,
                                                          Expression<Func<TSearchResult, bool>> constraints,
                                                          bool paginated,
                                                          int pageNumber,
                                                          int pageSize,
                                                          CancellationToken cancellationToken)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await GetSearchQuery(filters, extendedSearch, constraints, paginated, pageNumber, pageSize, context).ToListAsync(cancellationToken);
         }
      }

      protected IQueryable<TSearchResult> GetSearchQuery(List<string> filters,
                                                         bool extendedSearch,
                                                         Expression<Func<TSearchResult, bool>> constraints,
                                                         bool paginated,
                                                         int pageNumber,
                                                         int pageSize,
                                                         DbContext context)
      {
         var query = context.Set<TSearchResult>().AsNoTracking();

         query = query.OrderByDescending(e => e.DomainObjectId).Where(constraints);

         foreach (var filterConstraint in GetFilterConstraints(filters, extendedSearch))
         {
            query = query.Where(filterConstraint);
         }

         if (paginated)
         {
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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

      private void LogSearchAsync(string methodName, List<string> filters, bool extendedSearch)
      {
         if (filters == null || !filters.Any())
         {
            _logger.LogInformation("{Type}, {MethodName}, filters : {Filters}, extendedSearch : {ExtendedSearch}",
            GetType(), methodName, "none", extendedSearch.ToString());
         }
         else
         {
            _logger.LogInformation("{Type}, {MethodName}, filters : {Filters}, extendedSearch : {ExtendedSearch}",
            GetType(), methodName, string.Join(',', filters), extendedSearch.ToString());
         }
      }

      #endregion
   }
}
