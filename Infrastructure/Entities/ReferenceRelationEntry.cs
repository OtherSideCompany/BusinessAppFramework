using Application.Interfaces;
using Application.Relations;
using Domain;
using Domain.DomainObjects;
using System;
using System.Reflection;

namespace Infrastructure.Entities
{
   public class ReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity> : IReferenceRelationEntry
       where TSourceDomainObject : DomainObject
       where TTargetDomainObject : DomainObject
       where TSourceEntity : IEntity
       where TTargetEntity : class, IEntity
   {
      private StringKey _relationKey;

      public StringKey RelationKey => _relationKey;
      public Type SourceDomainObjectType => typeof(TSourceDomainObject);
      public Type TargetDomainObjectType => typeof(TTargetDomainObject);
      public Type SourceEntityType => typeof(TSourceEntity);
      public Type TargetEntityType => typeof(TTargetEntity);
      public PropertyInfo DomainProperty { get; }
      public PropertyInfo EntityIdProperty { get; }

      public ReferenceRelationEntry(
         StringKey key,
         PropertyInfo domainProperty,
         PropertyInfo entityIdProperty)
      {
         _relationKey = key;

         DomainProperty = domainProperty;
         EntityIdProperty = entityIdProperty;
      }
   }
}
