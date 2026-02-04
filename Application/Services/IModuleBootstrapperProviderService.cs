using Application.Interfaces;
using Domain;

namespace Application.Services
{
   public interface IModuleBootstrapperProviderService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModule> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(StringKey key);
   }
}
