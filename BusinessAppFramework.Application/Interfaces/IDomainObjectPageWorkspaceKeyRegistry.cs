using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDomainObjectPageWorkspaceKeyRegistry
   {
      void Register<T>(string workspaceKey) where T : DomainObject;
      string GetPageWorkspaceKey(Type domainObjectType);
      string GetPageWorkspaceKey<TDomainObject>();
   }
}
