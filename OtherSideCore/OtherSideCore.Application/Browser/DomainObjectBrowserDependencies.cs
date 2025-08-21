using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.Services;


namespace OtherSideCore.Application.Browser
{
   public class DomainObjectBrowserDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public ILoggerFactory LoggerFactory { get; }
      public IUserContext UserContext { get; }
      public IGlobalDataService GlobalDataService { get; }
      public IDomainObjectQueryServiceFactory DomainObjectQueryServiceFactory { get; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowserDependencies(
         ILoggerFactory loggerFactory,
         IUserContext userContext,
         IGlobalDataService globalDataService,
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory)
      {
         LoggerFactory = loggerFactory;
         UserContext = userContext;
         GlobalDataService = globalDataService;
         DomainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectSearchFactory = domainObjectSearchFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
