using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public interface IDomainObjectInteractionService
   {
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(Type type);
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      IDomainObjectEditorViewModel? CreateDomainObjectDetailsEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel? CreateDomainObjectDetailsEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);
      IDomainObjectTreeViewNode CreateDomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel);
      DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
      Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectBrowserAsync(Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
   }
}
