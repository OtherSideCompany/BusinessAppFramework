namespace BusinessAppFramework.Application.DomainObjectChains
{
    public interface IDomainObjectChainService
    {
        Task<DomainObjectChain> GetChainAsync(Type domainObjectType, int domainObjectId, DomainObjectChainDirection direction, CancellationToken ct = default);
    }
}
