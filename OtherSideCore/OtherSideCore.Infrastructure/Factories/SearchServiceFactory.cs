using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using System;

namespace OtherSideCore.Infrastructure.Factories
{
    public class SearchServiceFactory : ISearchServiceFactory
    {
        #region Fields

        private IServiceProvider _serviceProvider;

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public SearchServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        public ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new()
        {
            return _serviceProvider.GetRequiredService<ISearchService<TSearchResult>>();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
