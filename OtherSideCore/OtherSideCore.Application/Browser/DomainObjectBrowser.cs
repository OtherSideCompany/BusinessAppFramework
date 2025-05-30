
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

      protected IUserContext _userContext;
      protected ILoggerFactory _loggerFactory;
      protected IUserDialogService _userDialogService;

      #endregion

      #region Properties

      public DomainObjectSearch<TSearchResult> DomainObjectSearch { get; private set; }      
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; private set; }
      public IGlobalDataService GlobalDataService { get; private set; }
      public IDomainObjectQueryServiceFactory DomainObjectQueryServiceFactory { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowser(ILoggerFactory loggerFactory,
                                 IUserContext userContext,
                                 IUserDialogService userDialogService,
                                 IGlobalDataService globalDataService,
                                 IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
                                 IDomainObjectServiceFactory domainObjectServiceFactory,
                                 IDomainObjectSearchFactory domainObjectSearchFactory)
      {
         _userContext = userContext;
         _loggerFactory = loggerFactory;
         _userDialogService = userDialogService;
         DomainObjectSearch = (DomainObjectSearch<TSearchResult>)domainObjectSearchFactory.CreateDomainObjectSearch<TSearchResult>(domainObjectQueryServiceFactory);
         GlobalDataService = globalDataService;
         DomainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
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
