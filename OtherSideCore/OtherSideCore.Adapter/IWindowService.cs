
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      object ShowSubWindow();
      void ShowMainWindow<T>() where T : MainWindowViewModel;
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();      
      ViewDescriptionBase GetDescription(ViewBaseViewModel viewBaseViewModel);
      void ShowDomainObjectSearchView(DomainObjectViewModel domainObjectViewModel, WorkspaceViewModel workspaceViewModel, DisplayType displayType);
      void ShowDomainObjectEditorView(IDomainObjectEditorViewModel editorViewModel, DisplayType displayType);
      Task ShowDomainObjectSelectorViewAsync(IDomainObjectSelectorViewModel domainObjectSelectorViewModel, DisplayType displayType);
      void ShowDomainObjectTreeView(DomainObjectTreeViewModel domainObjectTreeViewModel, Type domainObjectType, DisplayType displayType);
   }
}
