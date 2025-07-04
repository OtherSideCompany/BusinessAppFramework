
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Browser
{
   public class DomainObjectBrowser<TDomainObject, TSearchResult> where TDomainObject : DomainObject, new()
                                                                  where TSearchResult : DomainObjectSearchResult, new()                
   {
      #region Fields

      protected DomainObjectBrowserDependencies _domainObjectBrowserDependencies;

      #endregion

      #region Properties

      public DomainObjectSearch<TSearchResult> DomainObjectSearch { get; private set; }      
      public IDomainObjectServiceFactory DomainObjectServiceFactory => _domainObjectBrowserDependencies.DomainObjectServiceFactory;
      public IGlobalDataService GlobalDataService => _domainObjectBrowserDependencies.GlobalDataService;
      public IDomainObjectQueryServiceFactory DomainObjectQueryServiceFactory => _domainObjectBrowserDependencies.DomainObjectQueryServiceFactory;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowser(DomainObjectBrowserDependencies domainObjectBrowserDependencies)
      {
         _domainObjectBrowserDependencies = domainObjectBrowserDependencies;

         DomainObjectSearch = (DomainObjectSearch<TSearchResult>)domainObjectBrowserDependencies.DomainObjectSearchFactory.CreateDomainObjectSearch<TSearchResult>(DomainObjectQueryServiceFactory);
      }

      #endregion

      #region Public Methods

      public virtual async Task InitializeAsync(List<string> filters)
      {
         await DomainObjectSearch.PaginatedSearchAsync(true, false, filters);
      }
      

      public virtual void Dispose()
      {
         DomainObjectSearch.Dispose();
      }

      #endregion

      #region Private Methods


      #endregion
   }
}
