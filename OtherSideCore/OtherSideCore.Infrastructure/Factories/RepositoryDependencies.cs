using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Relations;

namespace OtherSideCore.Infrastructure.Factories
{
   public class RepositoryDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDbContextFactory<DbContext> DbContextFactory { get; }
      public IMapper Mapper { get; }
      public ILoggerFactory LoggerFactory { get; }
      public IDomainObjectReferenceFactory DomainObjectReferenceFactory { get; }
      public IDomainObjectReferenceMapFactory ReferenceMapFactory { get; }
      public IRelationResolver RelationResolver { get; }
      public IDomainObjectEntityTypeMap DomainObjectEntityTypeMap { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryDependencies(
         IDbContextFactory<DbContext> dbContextFactory,
         IMapper mapper,
         ILoggerFactory loggerFactory,
         IDomainObjectReferenceFactory domainObjectReferenceFactory,
         IDomainObjectReferenceMapFactory referenceMapFactory,
         IRelationResolver parentChildRelationResolver,
         IDomainObjectEntityTypeMap domainObjectEntityTypeMap)
      {
         DbContextFactory = dbContextFactory;
         Mapper = mapper;
         LoggerFactory = loggerFactory;
         DomainObjectReferenceFactory = domainObjectReferenceFactory;
         ReferenceMapFactory = referenceMapFactory;
         RelationResolver = parentChildRelationResolver;
         DomainObjectEntityTypeMap = domainObjectEntityTypeMap;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
