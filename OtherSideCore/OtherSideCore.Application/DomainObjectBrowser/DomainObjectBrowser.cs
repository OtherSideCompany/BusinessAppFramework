
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Application.Views;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public class DomainObjectBrowser<T> : Workspace where T : DomainObject, new()
   {

      #region Fields

      

      #endregion

      #region Properties

      public DomainObjectSearch<T> DomainObjectSearch { get; private set; }
      public List<DomainObjectBrowser<T>> NestedDomainObjectBrowser { get; private set; }

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
                                 IDomainObjectSearchFactory domainObjectSearchFactory) : 
         base(loggerFactory, 
              userContext, 
              userDialogService, 
              globalDataService, 
              domainObjectQueryServiceFactory, 
              domainObjectServiceFactory)
      {
         DomainObjectSearch = (DomainObjectSearch<T>)domainObjectSearchFactory.CreateDomainObjectSearch<T>(domainObjectQueryServiceFactory);
         NestedDomainObjectBrowser = new List<DomainObjectBrowser<T>>();
      }

      #endregion

      #region Public Methods

      public override async Task InitializeAsync()
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

         DomainObjectSearch.AddSearchResult(domainObject);

         return domainObject;
      }

      public async Task DeleteAsync(T domainObject)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<T>();

         await domainObjectService.DeleteAsync(domainObject);
         DomainObjectSearch.RemoveSearchResult(domainObject);
      }

      public async Task DeleteAsync(List<T> domainObjects)
      {
         var domainObjectService = DomainObjectServiceFactory.CreateDomainObjectService<T>();

         foreach (var domainObject in domainObjects)
         {
            await domainObjectService.DeleteAsync(domainObject);
            DomainObjectSearch.RemoveSearchResult(domainObject);
         }
      }

      public override void Dispose()
      {
         DomainObjectSearch.Dispose();
         NestedDomainObjectBrowser.ForEach(domainObjectBrowser => domainObjectBrowser.Dispose());
      }

      #endregion

      #region Private Methods
      


      #endregion
   }
}
