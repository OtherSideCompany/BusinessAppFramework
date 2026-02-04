using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDomainObjectPageWorkspaceKeyRegistry
   {
      void Register<T>(StringKey workspaceKey) where T : DomainObject;
      StringKey GetPageWorkspaceKey(Type domainObjectType);
      StringKey GetPageWorkspaceKey<TDomainObject>();
   }
}
