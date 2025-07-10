using OtherSideCore.Adapter.Relations;
using System;
using System.Collections.Generic;

namespace OtherSideCore.Infrastructure.Mapping
{
   public class DomainObjectEntityTypeMap : IDomainObjectEntityTypeMap
   {
      #region Fields

      private readonly Dictionary<Type, Type> _domainToEntity = new();
      private readonly Dictionary<Type, Type> _entityToDomain = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEntityTypeMap()
      {

      }

      #endregion

      #region Public Methods

      public void Register(Type domainType, Type entityType)
      {
         _domainToEntity[domainType] = entityType;
         _entityToDomain[entityType] = domainType;
      }

      public Type GetEntityType(Type domainType) =>
        _domainToEntity.TryGetValue(domainType, out var entityType)
            ? entityType
            : throw new InvalidOperationException($"No entity mapped for domain type {domainType.Name}");

      public Type GetDomainType(Type entityType) =>
          _entityToDomain.TryGetValue(entityType, out var domainType)
              ? domainType
              : throw new InvalidOperationException($"No domain mapped for entity type {entityType.Name}");

      #endregion

      #region Private Methods



      #endregion
   }
}
