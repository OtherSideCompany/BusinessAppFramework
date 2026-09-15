namespace BusinessAppFramework.Application.Relations
{
   public interface IDomainObjectTypeMap
   {
      void Register(Type domainType, Type entityType, Type searchResultType);
      Type GetEntityTypeFromDomainObjectType(Type domainType);
      Type GetDomainTypeFromEntityType(Type entityType);
      Type GetSearchResultTypeFromDomainType(Type domainType);
      void RegisterSubstitution(Type domainType, Type substituteDomainType);
      Type ResolveDomainType(Type domainType);
   }
}
