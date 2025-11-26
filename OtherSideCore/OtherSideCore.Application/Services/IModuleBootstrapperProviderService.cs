using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Services
{
   public interface IModuleBootstrapperProviderService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModule> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(StringKey key);
   }
}
