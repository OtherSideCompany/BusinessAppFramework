using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
    public interface IDomainObjectSearchResultFactory
    {
        DomainObjectSearchResult CreateSearchResult<T>(int domainObjectId) where T : DomainObject, new();
    }
}
