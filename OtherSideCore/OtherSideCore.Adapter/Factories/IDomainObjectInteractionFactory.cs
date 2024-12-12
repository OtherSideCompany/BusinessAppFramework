using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Factories
{
   public interface IDomainObjectInteractionFactory
   {
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();
      IDomainObjectTreeViewNode CreateDomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel);
      DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
   }
}
