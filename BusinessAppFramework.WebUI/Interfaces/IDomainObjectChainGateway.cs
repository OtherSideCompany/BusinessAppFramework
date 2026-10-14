using BusinessAppFramework.Application.DomainObjectChains;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IDomainObjectChainGateway
    {
        Task<DomainObjectChain> GetChainAsync(string domainObjectTypeKey, int domainObjectId, DomainObjectChainDirection direction, CancellationToken cancellationToken = default);
    }
}
