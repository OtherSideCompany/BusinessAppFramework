using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   public class DomainObjectPageWorkspaceKeyRegistry : IDomainObjectPageWorkspaceKeyRegistry
   {
      private readonly Dictionary<Type, string> _map = new();

      public void Register<T>(string workspaceKey) where T : DomainObject
      {
         if (_map.ContainsKey(typeof(T)))
            throw new InvalidOperationException($"Page workspace already registered for {typeof(T).Name}.");

         _map[typeof(T)] = workspaceKey;
      }

      public string GetPageWorkspaceKey(Type domainObjectType)
      {
         if (_map.TryGetValue(domainObjectType, out var key))
            return key;

         throw new InvalidOperationException($"No page workspace registered for {domainObjectType.Name}.");
      }

      public string GetPageWorkspaceKey<TDomainObject>() => GetPageWorkspaceKey(typeof(TDomainObject));
   }
}
