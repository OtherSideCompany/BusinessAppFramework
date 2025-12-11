using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Reflection;

namespace OtherSideCore.Application.Factories
{
    public class DomainObjectServiceFactory : IDomainObjectServiceFactory
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public DomainObjectServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        public IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new()
        {
            return _serviceProvider.GetRequiredService<IDomainObjectService<T>>();
        }

        public object CreateDomainObjectService(Type domainObjectType)
        {
            var serviceType = typeof(IDomainObjectService<>).MakeGenericType(domainObjectType);
            return _serviceProvider.GetRequiredService(serviceType);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
