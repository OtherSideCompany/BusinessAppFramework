using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectInteractionFactory
   {
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      IDomainObjectTreeViewNode CreateDomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel);
      DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();
   }
}
