
namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      object ShowSubWindow();
      void ShowMainWindow();
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();
      void ShowDomainObjectViewModelInSubWindow(DomainObjectViewModel domainObjectViewModel);
   }
}
