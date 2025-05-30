using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Infrastructure.Factories
{
   public abstract class SearchServiceFactory : ISearchServiceFactory
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected ILoggerFactory _loggerFactory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SearchServiceFactory(
         IDbContextFactory<DbContext> dbContextFactory,
         ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _loggerFactory = loggerFactory;
      }

      #endregion

      #region Public Methods

      public abstract ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new();

      #endregion

      #region Private Methods



      #endregion
   }
}
