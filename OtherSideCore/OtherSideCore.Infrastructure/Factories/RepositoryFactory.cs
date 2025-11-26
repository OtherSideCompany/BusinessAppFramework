using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Repository;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using System;
namespace OtherSideCore.Infrastructure.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties


        #endregion

        #region Commands



        #endregion

        #region Constructor

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        #endregion

        #region Public Methods

        public IRepository<T> CreateRepository<T>() where T : DomainObject
        {
            return _serviceProvider.GetRequiredService<IRepository<T>>();
        }

        public object CreateRepository(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
