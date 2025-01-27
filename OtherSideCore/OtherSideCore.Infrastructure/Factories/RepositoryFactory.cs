using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Repository;
using OtherSideCore.Domain.DomainObjects;
namespace OtherSideCore.Infrastructure.Factories
{
   public abstract class RepositoryFactory : IRepositoryFactory
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected IMapper _mapper;
      protected ILoggerFactory _loggerFactory;
      protected IDomainObjectReferenceFactory _domainObjectReferenceFactory;
      protected IDomainObjectReferenceMapFactory _referenceMapFactory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryFactory(
         IDbContextFactory<DbContext> dbContextFactory, 
         IMapper mapper, 
         ILoggerFactory loggerFactory, 
         IDomainObjectReferenceFactory domainObjectReferenceFactory,
         IDomainObjectReferenceMapFactory referenceMapFactory)
      {
         _dbContextFactory = dbContextFactory;
         _mapper = mapper;
         _loggerFactory = loggerFactory;
         _domainObjectReferenceFactory = domainObjectReferenceFactory;
         _referenceMapFactory = referenceMapFactory;
      }


      #endregion

      #region Public Methods

      public abstract IRepository<T> CreateRepository<T>() where T : DomainObject, new();

      #endregion

      #region Private Methods



      #endregion
   }
}
