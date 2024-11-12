using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public class DomainObjectSelector<T> : DomainObjectBrowser<T> where T : DomainObject, new()
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSelector(ILoggerFactory loggerFactory, 
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
              domainObjectServiceFactory, 
              domainObjectSearchFactory)
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion

   }
}
