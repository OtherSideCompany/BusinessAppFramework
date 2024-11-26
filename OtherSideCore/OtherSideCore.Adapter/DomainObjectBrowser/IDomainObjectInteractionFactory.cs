using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public interface IDomainObjectInteractionFactory
   {
      IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();
      IDomainObjectBrowserViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      DomainObjectTreeViewNode CreateTreeViewNode(DomainObjectViewModel domainObjectViewModel, DomainObjectTreeViewModel domainObjectTreeViewModel);
   }
}
