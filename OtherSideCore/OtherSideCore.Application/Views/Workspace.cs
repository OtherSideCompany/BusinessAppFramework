using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Views
{
   public abstract class Workspace : ViewBase
   {
      #region Fields
            

      #endregion

      #region Properties

      public virtual bool HasUnsavedChanges => false;

      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; private set; }
      public IGlobalDataService GlobalDataService { get; private set; }
      public IDomainObjectQueryServiceFactory DomainObjectQueryServiceFactory { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      protected Workspace(ILoggerFactory loggerFactory,
                          IUserContext userContext,
                          IUserDialogService userDialogService,
                          IGlobalDataService globalDataService,
                          IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
                          IDomainObjectServiceFactory domainObjectServiceFactory) :
         base(loggerFactory,
              userContext,
              userDialogService)
      {
         GlobalDataService = globalDataService;
         DomainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
