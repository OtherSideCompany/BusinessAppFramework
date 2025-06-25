
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
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

      public async Task<TDomainObject> CreateAsync(DomainObject? parent)
      {
         var domainObject = new TDomainObject();

         return await CreateAsync(domainObject, parent);
      }

      public async Task<TDomainObject> CreateAsync(TDomainObject domainObject, DomainObject? parent)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();

         await domainObjectService.CreateAsync(domainObject, parent);

         await DomainObjectSearch.AddSearchResultAsync(domainObject.Id);

         return domainObject;
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
