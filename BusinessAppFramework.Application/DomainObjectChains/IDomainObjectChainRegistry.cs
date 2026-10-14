using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.DomainObjectChains
{
    public interface IDomainObjectChainRegistry
    {
        void Link<TFrom, TTo>(string relationKey) where TFrom : DomainObject where TTo : DomainObject;
        IReadOnlyList<DomainObjectChainLink> GetLinksFrom(Type domainObjectType);
        IReadOnlyList<DomainObjectChainLink> GetLinksTo(Type domainObjectType);
        Type? ResolveType(Type domainObjectType);
        Type? ResolveType(string domainObjectTypeKey);
    }
}
