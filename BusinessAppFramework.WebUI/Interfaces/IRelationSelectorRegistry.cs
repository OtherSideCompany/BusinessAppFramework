using BusinessAppFramework.Domain;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IRelationSelectorRegistry
   {
      void Register(string key, string value);
      string Resolve(string key);
      bool TryResolve(string key, out string value);
   }
}
