using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
    public interface IDomainObjectSearchFactory
    {
        IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObject, new();
        IDomainObjectTreeSearch CreateDomainObjectTreeSearch<T>(IDomainObjectServiceFactory domainObjectServiceFactory) where T : DomainObject, new();
    }
}
