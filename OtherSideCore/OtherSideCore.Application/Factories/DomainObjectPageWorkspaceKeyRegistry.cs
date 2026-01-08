using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
    public class DomainObjectPageWorkspaceKeyRegistry : IDomainObjectPageWorkspaceKeyRegistry
    {
        private readonly Dictionary<Type, StringKey> _map = new();

        public void Register<T>(StringKey workspaceKey) where T : DomainObject
        {
            if (_map.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"Page workspace already registered for {typeof(T).Name}.");

            _map[typeof(T)] = workspaceKey;
        }

        public StringKey GetPageWorkspaceKey(Type domainObjectType)
        {
            if (_map.TryGetValue(domainObjectType, out var key))
                return key;

            throw new InvalidOperationException($"No page workspace registered for {domainObjectType.Name}.");
        }

        public StringKey GetPageWorkspaceKey<TDomainObject>() => GetPageWorkspaceKey(typeof(TDomainObject));
    }
}
