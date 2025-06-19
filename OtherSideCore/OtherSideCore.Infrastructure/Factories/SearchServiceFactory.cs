using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using System;

namespace OtherSideCore.Infrastructure.Factories
{
   public class SearchServiceFactory : TypeBasedFactory, ISearchServiceFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public ISearchService<TSearchResult> CreateSearchService<TSearchResult>() where TSearchResult : DomainObjectSearchResult, new()
      {
         return (ISearchService<TSearchResult>)CreateFromType(typeof(TSearchResult));
      }

      public void Register<TSearchResult>(Func<ISearchService<TSearchResult>> factory) where TSearchResult : DomainObjectSearchResult, new()
      {
         Register(typeof(TSearchResult), args => factory());
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
