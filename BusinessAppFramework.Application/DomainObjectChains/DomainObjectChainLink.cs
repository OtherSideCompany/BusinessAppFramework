namespace BusinessAppFramework.Application.DomainObjectChains
{
    public record DomainObjectChainLink(Type FromType, Type ToType, string RelationKey);
}
