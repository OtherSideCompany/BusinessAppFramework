using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter
{
   public abstract class DomainObjectInteractionService : IDomainObjectInteractionService
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
      protected IDomainObjectSearchResultViewModelFactory _domainObjectSearchResultViewModelFactory;
      protected IDomainObjectReferenceMapFactory _referenceMapFactory;


      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectInteractionService(
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
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
         IDomainObjectReferenceMapFactory referenceMapFactory)
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
         _domainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
         _referenceMapFactory = referenceMapFactory;
      }

      #endregion

      #region Public Methods

      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(Type type);

      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();

      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();

      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);

      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();

      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();

      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);

      public abstract List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);

      public virtual async Task<IDomainObjectTreeViewNode> CreateDomainObjectTreeViewNodeAsync(DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectType = domainObjectViewModel.DomainObject.GetType();
         var genericType = typeof(DomainObjectTreeViewNode<>).MakeGenericType(domainObjectType);

         var treeViewNode = (IDomainObjectTreeViewNode)Activator.CreateInstance(genericType, domainObjectViewModel, _userDialogService, this, _domainObjectServiceFactory);
         await treeViewNode.InitializeAsync();

         return treeViewNode;
      }

      public abstract DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();

      public abstract Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectBrowserAsync(Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);


      #endregion

      #region Private Methods



      #endregion
   }
}
