using OtherSideCore.Application;
using System.Windows;

namespace OtherSideCore.Adapter.Services
{
   public interface IViewLocatorService
   {
      void Register(StringKey viewKey, Type viewType);
      object ResolveView(StringKey viewKey, object viewModel);
   }
}
