using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public interface IDomainObjectInteractionService
   {
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      IDomainObjectTreeViewNode CreateDomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel);
      DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
      Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      Task DisplayDomainObjectEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      Task DisplayDomainObjectEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
   }
}
