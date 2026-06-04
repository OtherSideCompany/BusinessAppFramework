using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IModuleBootstrapperProviderService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModuleBootstrapper> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(string key);
   }
}
