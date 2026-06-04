using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDomainObjectBrowserWorkspaceRegistry
   {
      void Register<T>(string browserKey) where T : DomainObject;
      string Resolve<T>() where T : DomainObject;
      bool TryResolve<T>(out string browserKey) where T : DomainObject;
   }
}
