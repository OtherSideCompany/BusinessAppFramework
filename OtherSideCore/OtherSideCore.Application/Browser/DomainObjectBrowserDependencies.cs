using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Browser
{
   public class DomainObjectBrowserDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public ILoggerFactory LoggerFactory { get; }
      public IUserContext UserContext { get; }
      public IUserDialogService UserDialogService { get; }
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
         IUserDialogService userDialogService,
         IGlobalDataService globalDataService,
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory)
      {
         LoggerFactory = loggerFactory;
         UserContext = userContext;
         UserDialogService = userDialogService;
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
