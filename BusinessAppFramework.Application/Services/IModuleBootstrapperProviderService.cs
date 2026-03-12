using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Services
{
   public interface IModuleBootstrapperPurchaseService
   {
      List<IModuleBootstrapper> GetModules();
      List<ISearchModule> GetSearchModules();
      IModuleBootstrapper? GetModuleByKey(StringKey key);
   }
}
