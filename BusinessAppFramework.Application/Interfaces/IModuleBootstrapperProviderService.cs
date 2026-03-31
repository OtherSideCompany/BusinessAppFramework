using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IModuleBootstrapperProviderService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModule> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(StringKey key);
   }
}
