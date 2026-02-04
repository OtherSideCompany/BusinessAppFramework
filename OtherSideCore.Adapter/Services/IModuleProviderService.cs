namespace OtherSideCore.Adapter
{
   public interface IModuleProviderService
   {
      List<IModule> GetModules();
      List<ISearchModule> GetSearchModules();
   }
}
