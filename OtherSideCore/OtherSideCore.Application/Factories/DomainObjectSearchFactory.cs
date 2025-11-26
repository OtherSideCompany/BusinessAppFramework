using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Application.Factories
{
    public class DomainObjectSearchFactory : IDomainObjectSearchFactory
    {
        #region Fields

        private IServiceProvider _serviceProvider;


        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public DomainObjectSearchFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        public IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObjectSearchResult, new()
        {
            return _serviceProvider.GetRequiredService<IDomainObjectSearch<T>>();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
