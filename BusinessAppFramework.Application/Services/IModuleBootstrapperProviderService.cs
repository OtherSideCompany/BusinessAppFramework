using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Services
{
   public interface IModuleBootstrapperProviderService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModule> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(StringKey key);
   }
}
