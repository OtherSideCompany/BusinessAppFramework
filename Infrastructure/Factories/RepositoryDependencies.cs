using Application.Relations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Factories
{
   public class RepositoryDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDbContextFactory<DbContext> DbContextFactory { get; }
      public IMapper Mapper { get; }
      public ILoggerFactory LoggerFactory { get; }
      public IRelationResolver RelationResolver { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryDependencies(
         IDbContextFactory<DbContext> dbContextFactory,
         IMapper mapper,
         ILoggerFactory loggerFactory,
         IRelationResolver parentChildRelationResolver)
      {
         DbContextFactory = dbContextFactory;
         Mapper = mapper;
         LoggerFactory = loggerFactory;
         RelationResolver = parentChildRelationResolver;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
