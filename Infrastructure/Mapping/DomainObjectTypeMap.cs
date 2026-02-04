using Application.Relations;
using System;
using System.Collections.Generic;

namespace Infrastructure.Mapping
{
   public class DomainObjectTypeMap : IDomainObjectTypeMap
   {
      #region Fields

      private readonly Dictionary<Type, Type> _domainToEntity = new();
      private readonly Dictionary<Type, Type> _entityToDomain = new();
      private readonly Dictionary<Type, Type> _domainToSearchResult = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTypeMap()
      {

      }

      #endregion

      #region Public Methods

      public void Register(Type domainType, Type entityType, Type searchResultType)
      {
         _domainToEntity[domainType] = entityType;
         _entityToDomain[entityType] = domainType;
         _domainToSearchResult[domainType] = searchResultType;
      }

      public Type GetEntityTypeFromDomainObjectType(Type domainType) =>
        _domainToEntity.TryGetValue(domainType, out var entityType)
            ? entityType
            : throw new InvalidOperationException($"No entity type mapped for domain type {domainType.Name}");

      public Type GetDomainTypeFromEntityType(Type entityType) =>
          _entityToDomain.TryGetValue(entityType, out var domainType)
              ? domainType
              : throw new InvalidOperationException($"No domain type mapped for entity type {entityType.Name}");

      public Type GetSearchResultTypeFromDomainType(Type domainType) =>
          _domainToSearchResult.TryGetValue(domainType, out var searchResultType)
              ? searchResultType
              : throw new InvalidOperationException($"No search result type mapped for domain type {domainType.Name}");


      #endregion

      #region Private Methods



      #endregion
   }
}
