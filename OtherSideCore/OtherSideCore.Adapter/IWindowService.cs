
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Views;

namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      void ShowSubWindow(object content, string windowName);
      void ShowView(object view, string windowName, DisplayType displayType);
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();   
      void ShowDomainObjectSearchView(DomainObjectViewModel domainObjectViewModel, Workspace workspaceViewModel, DisplayType displayType);
      void ShowDomainObjectSearchView(Type domainObjectType, Workspace workspaceViewModel, DisplayType displayType);
      void ShowDomainObjectDetailsEditorView(IDomainObjectEditorViewModel editorViewModel, DisplayType displayType);
      Task ShowDomainObjectSelectorViewAsync(IDomainObjectSelectorViewModel domainObjectSelectorViewModel, DisplayType displayType);
      void ShowDomainObjectTreeViewWorkspace(DomainObjectTreeViewModel domainObjectTreeViewModel, Type domainObjectType, DisplayType displayType);
      void ShowDomainObjectReferenceSelectors(List<DomainObjectReferenceSelectorViewModel> domainObjectReferenceSelectorViewModels, DisplayType displayType);
   }
}
