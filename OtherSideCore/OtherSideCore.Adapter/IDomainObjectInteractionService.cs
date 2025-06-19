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
    public interface IDomainObjectInteractionService
   {
      void RegisterDomainObjectBrowserViewModel(StringKey key, Func<IDomainObjectBrowserViewModel> factory);
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel(StringKey key);

      void RegisterDomainObjectEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory);
      void RegisterSearchResultEditorMapping<TSearchResult>(Type domainObjectType, StringKey editorKey) where TSearchResult : DomainObjectSearchResult;
      void RegisterTreeNodeViewModelEditorMapping(Type treeNodeViewModelType, StringKey editorKey);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObject domainObject);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);

      void RegisterDomainObjectSelectorViewModel(StringKey key, Func<IDomainObjectSelectorViewModel> factory);
      IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(StringKey key, Type type);

      void RegisterTreeViewModel(StringKey key, Func<DomainObjectTree, DomainObjectTreeViewModel> factory);
      DomainObjectTreeViewModel CreateTreeViewModel(StringKey key);

      void RegisterTree(StringKey key, Func<IDomainObjectTree> factory);
      IDomainObjectTree CreateTree(StringKey key, IDomainObjectServiceFactory domainObjectServiceFactory);




      //OLD
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(Type type);
      Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectTreeNodeViewModel> CreateDomainObjectTreeViewNodeAsync(DomainObjectViewModel domainObjectViewModel, IUserDialogService userDialogService, IDomainObjectServiceFactory domainObjectServiceFactory);
      //DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
      Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectBrowserAsync(Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
   }
}
