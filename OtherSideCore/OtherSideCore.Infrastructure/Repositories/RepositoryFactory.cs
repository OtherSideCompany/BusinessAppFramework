using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public abstract class RepositoryFactory : IRepositoryFactory
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory;
      protected IMapper _mapper;
      protected ILoggerFactory _loggerFactory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryFactory(IDbContextFactory<DbContext> dbContextFactory, IMapper mapper, ILoggerFactory loggerFactory)
      {
         _dbContextFactory = dbContextFactory;
         _mapper = mapper;
         _loggerFactory = loggerFactory;
      }


      #endregion

      #region Public Methods

      public abstract IRepository<T> CreateRepository<T>() where T : DomainObject, new();

      #endregion

      #region Private Methods



      #endregion
   }
}
