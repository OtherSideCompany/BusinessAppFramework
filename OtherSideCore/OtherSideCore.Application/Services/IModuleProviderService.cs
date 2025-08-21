using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Services
{
   public interface IModuleProviderService
   {
      List<IModule> GetModules();
      List<ISearchModule> GetSearchModules();
      IModule? GetModuleByWorkspaceKey(StringKey key);
   }
}
