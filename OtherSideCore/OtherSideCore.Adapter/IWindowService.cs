
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      void ShowSubWindow(object content, string windowName);
      void ShowMainWindow<T>() where T : MainWindowViewModel;
      void ShowView(object view, string windowName, DisplayType displayType);
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();      
      ViewDescriptionBase GetDescription(ViewBaseViewModel viewBaseViewModel);
      void ShowDomainObjectSearchView(DomainObjectViewModel domainObjectViewModel, WorkspaceViewModel workspaceViewModel, DisplayType displayType);
      void ShowDomainObjectSearchView(Type domainObjectType, WorkspaceViewModel workspaceViewModel, DisplayType displayType);
      void ShowDomainObjectEditorView(IDomainObjectEditorViewModel editorViewModel, DisplayType displayType);
      Task ShowDomainObjectSelectorViewAsync(IDomainObjectSelectorViewModel domainObjectSelectorViewModel, DisplayType displayType);
      void ShowDomainObjectTreeViewWorkspace(DomainObjectTreeViewModel domainObjectTreeViewModel, Type domainObjectType, DisplayType displayType);
      void ShowDomainObjectReferenceSelectors(List<DomainObjectReferenceSelectorViewModel> domainObjectReferenceSelectorViewModels, DisplayType displayType);
   }
}
