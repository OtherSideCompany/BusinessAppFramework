using System.Windows;

namespace OtherSideCore.Presentation.Services
{
   public interface IViewLocatorService
   {
      void Register(Type viewModelType, Type viewType);
      object ResolveView(object viewModel);
   }
}
