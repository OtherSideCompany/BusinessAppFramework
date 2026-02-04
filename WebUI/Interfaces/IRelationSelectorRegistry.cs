using Domain;

namespace WebUI.Interfaces
{
   public interface IRelationSelectorRegistry
   {
      void Register(StringKey key, StringKey value);
      StringKey Resolve(StringKey key);
      bool TryResolve(StringKey key, out StringKey value);
   }
}
