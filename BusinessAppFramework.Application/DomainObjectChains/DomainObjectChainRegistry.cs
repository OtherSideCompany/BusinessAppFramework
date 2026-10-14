using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.DomainObjectChains
{
    public class DomainObjectChainRegistry : IDomainObjectChainRegistry
    {
        #region Fields

        private readonly IDomainObjectTypeMap _domainObjectTypeMap;
        private readonly List<DomainObjectChainLink> _links = new();
        private readonly Dictionary<string, Type> _typesByKey = new();

        #endregion

        #region Constructor

        public DomainObjectChainRegistry(IDomainObjectTypeMap domainObjectTypeMap)
        {
            _domainObjectTypeMap = domainObjectTypeMap;
        }

        #endregion

        #region Public Methods

        public void Link<TFrom, TTo>(string relationKey) where TFrom : DomainObject where TTo : DomainObject
        {
            _links.Add(new DomainObjectChainLink(typeof(TFrom), typeof(TTo), relationKey));

            _typesByKey[DomainObjectAggregateKeys.Type(typeof(TFrom))] = typeof(TFrom);
            _typesByKey[DomainObjectAggregateKeys.Type(typeof(TTo))] = typeof(TTo);
        }

        public IReadOnlyList<DomainObjectChainLink> GetLinksFrom(Type domainObjectType) => _links.Where(l => l.FromType == domainObjectType).ToList();

        public IReadOnlyList<DomainObjectChainLink> GetLinksTo(Type domainObjectType) => _links.Where(l => l.ToType == domainObjectType).ToList();

        public Type? ResolveType(Type domainObjectType)
        {
            for (var type = domainObjectType; type != null && type != typeof(DomainObject); type = type.BaseType)
            {
                if (_typesByKey.ContainsValue(type))
                    return type;
            }

            return null;
        }

        public Type? ResolveType(string domainObjectTypeKey)
        {
            if (_typesByKey.TryGetValue(domainObjectTypeKey, out var type))
                return type;

            var domainType = _domainObjectTypeMap.FindDomainType(domainObjectTypeKey);

            return domainType == null ? null : ResolveType(domainType);
        }

        #endregion
    }
}
