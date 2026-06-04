namespace BusinessAppFramework.Adapter.Interfaces
{
   public interface IWebApiModuleBootstrapper
   {
        List<(Type ControllerType, string RouteKey)> GetControllerDescriptions();
   }
}
