using Microsoft.Extensions.Logging;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public abstract class DomainObjectInteractionFactory : IDomainObjectInteractionFactory
   {
      #region Fields

      protected ILoggerFactory _loggerFactory;
      protected IUserContext _userContext;
      protected IUserDialogService _userDialogService;
      protected IGlobalDataService _globalDataService;
      protected IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected IDomainObjectSearchFactory _domainObjectSearchFactory;
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;
      protected IDomainObjectsSearchViewModelFactory _domainObjectsSearchViewModelFactory;
      protected IWindowService _windowService;
      protected IDomainObjectHierarchyService _domainObjectHierarchyService;
      protected IDomainObjectFileService _domainObjectFileService;


      #endregion

      #region Properties



      #endregion


      #region Constructor

      public DomainObjectInteractionFactory(
         ILoggerFactory loggerFactory,
         IUserContext userContext,
         IUserDialogService userDialogService,
         IGlobalDataService globalDataService,
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
         IWindowService windowService,
         IDomainObjectHierarchyService domainObjectHierarchyService,
         IDomainObjectFileService domainObjectFileService)
      {
         _loggerFactory = loggerFactory;
         _userContext = userContext;
         _userDialogService = userDialogService;
         _globalDataService = globalDataService;
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _domainObjectSearchFactory = domainObjectSearchFactory;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;
         _domainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         _windowService = windowService;
         _domainObjectHierarchyService = domainObjectHierarchyService;
         _domainObjectFileService = domainObjectFileService;
      }

      #endregion

      #region Public Methods

      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();

      public abstract IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);

      public abstract DomainObjectTreeViewNode CreateTreeViewNode(DomainObjectViewModel domainObjectViewModel, DomainObjectTreeViewModel domainObjectTreeViewModel);

      #endregion

      #region Private Methods



      #endregion
   }
}
