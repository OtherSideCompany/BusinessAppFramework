using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter.Factories
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
      protected IDomainObjectFileService _domainObjectFileService;
      protected IDomainObjectSearchResultFactory _domainObjectSearchResultFactory;
      protected IDomainObjectSearchResultViewModelFactory _domainObjectSearchResultViewModelFactory;


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
         IDomainObjectFileService domainObjectFileService,
         IDomainObjectSearchResultFactory domainObjectSearchResultFactory,
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory)
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
         _domainObjectFileService = domainObjectFileService;
         _domainObjectSearchResultFactory = domainObjectSearchResultFactory;
         _domainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
      }

      #endregion

      #region Public Methods

      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();

      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();

      public abstract IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);

      public virtual IDomainObjectTreeViewNode CreateDomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectType = domainObjectViewModel.DomainObject.GetType();
         var genericType = typeof(DomainObjectTreeViewNode<>).MakeGenericType(domainObjectType);

         return (IDomainObjectTreeViewNode)Activator.CreateInstance(genericType, domainObjectViewModel, _userDialogService, _windowService, this, _domainObjectServiceFactory);
      }

      public abstract DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();


      #endregion

      #region Private Methods



      #endregion
   }
}
