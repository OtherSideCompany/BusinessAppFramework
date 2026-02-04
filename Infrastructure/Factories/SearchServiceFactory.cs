using Application.Factories;
using Application.Search;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Factories
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

      public object CreateSearchService(Type domainObjectSearchResultType)
      {
         var serviceType = typeof(ISearchService<>).MakeGenericType(domainObjectSearchResultType);
         return _serviceProvider.GetRequiredService(serviceType);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
