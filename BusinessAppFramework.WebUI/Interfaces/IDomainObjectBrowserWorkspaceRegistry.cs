using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDomainObjectBrowserWorkspaceRegistry
   {
      void Register<T>(StringKey browserKey) where T : DomainObject;
      StringKey Resolve<T>() where T : DomainObject;
      bool TryResolve<T>(out StringKey browserKey) where T : DomainObject;
   }
}
