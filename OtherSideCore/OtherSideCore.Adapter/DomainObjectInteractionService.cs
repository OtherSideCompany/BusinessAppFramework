using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Tree;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
    public abstract class DomainObjectInteractionService : IDomainObjectInteractionService
   {
      #region Fields      

      protected DomainObjectInteractionServiceDependencies _domainObjectInteractionServiceDependencies;

      #endregion

      #region Properties

      public abstract Dictionary<StringKey, StringKey> SelectorToWorkspaceKeyMappings { get; }

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
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObject domainObject);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);
      public abstract void RegisterDomainObjectDetailsEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory);
      public abstract void RegisterSearchResultDetailsEditorMapping<TSearchResult>(Type domainObjectType, StringKey editorKey) where TSearchResult : DomainObjectSearchResult;
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObject domainObject);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);
      public abstract void RegisterSelectorToWorkspaceKeyMapping(StringKey selectorKey, StringKey editorKey);
      public abstract void RegisterDomainObjectSelectorViewModel(StringKey key, Func<IDomainObjectSelectorViewModel> factory);
      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(StringKey key);
      public abstract void RegisterTree(StringKey key, Func<IDomainObjectTree> factory);
      public abstract IDomainObjectTree CreateTree(StringKey key, IDomainObjectServiceFactory domainObjectServiceFactory);
      public abstract void RegisterTreeViewModel(StringKey key, Func<DomainObjectTree, DomainObjectTreeViewModel> factory);
      public abstract DomainObjectTreeViewModel CreateTreeViewModel(StringKey key);

      public abstract void RegisterTreeNodeViewModel(Type type, Func<DomainObjectViewModel, IDomainObjectTreeNodeViewModel> factory);
      public abstract Task<IDomainObjectTreeNodeViewModel> CreateDomainObjectTreeNodeViewModelAsync(DomainObjectViewModel domainObjectViewModel);

      // OLD
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);     
      public abstract Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);

      #endregion

      #region Private Methods



      #endregion
   }
}
