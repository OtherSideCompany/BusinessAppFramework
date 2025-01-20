
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Browser
{
    public class DomainObjectBrowser<T> where T : DomainObject, new()
   {
      #region Fields

      protected IUserContext _userContext;
      protected ILoggerFactory _loggerFactory;
      protected IUserDialogService _userDialogService;

      #endregion

      #region Properties

      public DomainObjectSearch<T> DomainObjectSearch { get; private set; }
      public List<DomainObjectBrowser<T>> NestedDomainObjectBrowser { get; private set; }
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
         DomainObjectSearch = (DomainObjectSearch<T>)domainObjectSearchFactory.CreateDomainObjectSearch<T>(domainObjectQueryServiceFactory);
         NestedDomainObjectBrowser = new List<DomainObjectBrowser<T>>();
         GlobalDataService = globalDataService;
         DomainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods

      public virtual async Task InitializeAsync()
      {
         await DomainObjectSearch.PaginatedSearchAsync(true, false, []);
      }

      public async Task<T> CreateAsync(DomainObject? parent)
      {
         var domainObject = new T();

         return await CreateAsync(domainObject, parent);
      }

      public async Task<T> CreateAsync(T domainObject, DomainObject? parent)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<T>();

         await domainObjectService.CreateAsync(domainObject, parent);

         await DomainObjectSearch.AddSearchResultAsync(domainObject.Id);

         return domainObject;
      }

      public async Task DeleteAsync(T domainObject)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<T>();

         await domainObjectService.DeleteAsync(domainObject);
         DomainObjectSearch.RemoveSearchResult(domainObject.Id);
      }

      public async Task DeleteAsync(List<T> domainObjects)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<T>();

         foreach (var domainObject in domainObjects)
         {
            await domainObjectService.DeleteAsync(domainObject);
            DomainObjectSearch.RemoveSearchResult(domainObject.Id);
         }
      }

      public virtual void Dispose()
      {
         DomainObjectSearch.Dispose();
         NestedDomainObjectBrowser.ForEach(domainObjectBrowser => domainObjectBrowser.Dispose());
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
