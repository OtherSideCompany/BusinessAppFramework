using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Tree;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
    public abstract class DomainObjectInteractionService : IDomainObjectInteractionService
   {
      #region Fields      

      protected DomainObjectInteractionServiceDependencies _domainObjectInteractionServiceDependencies;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectInteractionService(DomainObjectInteractionServiceDependencies domainObjectInteractionServiceDependencies)
      {
         _domainObjectInteractionServiceDependencies = domainObjectInteractionServiceDependencies;
      }

      #endregion

      #region Public Methods

      public abstract void RegisterDomainObjectBrowserViewModel(StringKey key, Func<IDomainObjectBrowserViewModel> factory);
      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel(StringKey key);
      public abstract void RegisterDomainObjectEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory);
      public abstract void RegisterSearchResultEditorMapping<TSearchResult>(Type domainObjectType, StringKey editorKey) where TSearchResult : DomainObjectSearchResult;
      public abstract void RegisterTreeNodeViewModelEditorMapping(Type treeNodeViewModelType, StringKey editorKey);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObject domainObject);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);
      public abstract void RegisterDomainObjectSelectorViewModel(StringKey key, Func<IDomainObjectSelectorViewModel> factory);
      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(StringKey key, Type type);
      public abstract void RegisterTree(StringKey key, Func<IDomainObjectTree> factory);
      public abstract IDomainObjectTree CreateTree(StringKey key, IDomainObjectServiceFactory domainObjectServiceFactory);
      public abstract void RegisterTreeViewModel(StringKey key, Func<DomainObjectTree, DomainObjectTreeViewModel> factory);
      public abstract DomainObjectTreeViewModel CreateTreeViewModel(StringKey key);

      public virtual async Task<IDomainObjectTreeNodeViewModel> CreateDomainObjectTreeViewNodeAsync(DomainObjectViewModel domainObjectViewModel, IUserDialogService userDialogService, IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         var domainObjectType = domainObjectViewModel.DomainObject.GetType();
         var genericType = typeof(DomainObjectTreeNodeViewModel<>).MakeGenericType(domainObjectType);

         var treeViewNode = (IDomainObjectTreeNodeViewModel)Activator.CreateInstance(genericType, domainObjectViewModel, userDialogService, this, domainObjectServiceFactory);
         await treeViewNode.InitializeAsync();

         return treeViewNode;
      }


      // OLD
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();

      //public abstract DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(Type type);

      public abstract List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);     

      

      public abstract Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectBrowserAsync(Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);

      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();

      #endregion

      #region Private Methods



      #endregion
   }
}
