
namespace OtherSideCore.Adapter
{
   public interface IWindowService
   {
      void ShowSubWindow(object content);
      void ShowMainWindow();
      void CloseWindow(object window);
      void ShowModal(object modalContent);
      void HideTopModal();
   }
}
