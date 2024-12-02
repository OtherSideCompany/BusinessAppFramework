
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Adapter.Views;

namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      object ShowSubWindow();
      void ShowMainWindow<T>() where T : MainWindowViewModel;
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();
      void ShowDomainObjectViewModelInSubWindow(DomainObjectViewModel domainObjectViewModel, WorkspaceViewModel workspaceViewModel);
      ViewDescriptionBase GetDescription(ViewBaseViewModel viewBaseViewModel);
   }
}
