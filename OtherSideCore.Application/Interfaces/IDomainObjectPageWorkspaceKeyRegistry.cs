using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Interfaces
{
    public interface IDomainObjectPageWorkspaceKeyRegistry
    {
        void Register<T>(StringKey workspaceKey) where T : DomainObject;
        StringKey GetPageWorkspaceKey(Type domainObjectType);
        StringKey GetPageWorkspaceKey<TDomainObject>();
    }
}
