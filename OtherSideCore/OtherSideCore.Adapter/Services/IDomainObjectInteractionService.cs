using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Application.Tree;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Services
{
    public interface IDomainObjectInteractionService
    {
        Dictionary<StringKey, StringKey> SelectorToWorkspaceKeyMappings { get; }

        void RegisterDomainObjectBrowserViewModel(StringKey key, Func<IDomainObjectBrowserViewModel> factory);
        IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel(StringKey key);

        void RegisterDomainObjectEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory);
        void RegisterSearchResultEditorMapping<TSearchResult>(Type domainObjectType, StringKey editorKey) where TSearchResult : DomainObjectSearchResult;
        void RegisterTreeNodeViewModelEditorMapping(Type treeNodeViewModelType, StringKey editorKey);
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
        Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
        Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObject domainObject);
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);

        void RegisterDomainObjectDetailsEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory);
        void RegisterSearchResultDetailsEditorMapping<TSearchResult>(Type domainObjectType, StringKey editorKey) where TSearchResult : DomainObjectSearchResult;
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel);
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult);
        Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel);
        Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObject domainObject);
        Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId);

        void RegisterSelectorToWorkspaceKeyMapping(StringKey selectorKey, StringKey editorKey);
        void RegisterDomainObjectSelectorViewModel(StringKey key, Func<IDomainObjectSelectorViewModel> factory);
        IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(StringKey key);

        void RegisterTreeViewModel(StringKey key, Func<DomainObjectTree, DomainObjectTreeViewModel> factory);
        DomainObjectTreeViewModel CreateTreeViewModel(StringKey key);

        void RegisterTree(StringKey key, Func<IDomainObjectTree> factory);
        IDomainObjectTree CreateTree(StringKey key, IDomainObjectServiceFactory domainObjectServiceFactory);

        void RegisterTreeNodeViewModel(Type type, Func<DomainObjectViewModel, IDomainObjectTreeNodeViewModel> factory);
        Task<IDomainObjectTreeNodeViewModel> CreateDomainObjectTreeNodeViewModelAsync(DomainObjectViewModel domainObjectViewModel);


        //OLD
        Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
        List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);
        Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
        Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
    }
}
