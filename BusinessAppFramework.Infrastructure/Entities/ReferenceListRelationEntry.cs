using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;
using System;
using System.Reflection;

namespace BusinessAppFramework.Infrastructure.Entities
{
   public class ReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity> : IReferenceListRelationEntry
       where TSourceDomainObject : DomainObject
       where TTargetDomainObject : DomainObject
       where TSourceEntity : IEntity
       where TTargetEntity : class, IEntity
   {
      private string _relationKey;

      public string RelationKey => _relationKey;
      public Type SourceDomainObjectType => typeof(TSourceDomainObject);
      public Type TargetDomainObjectType => typeof(TTargetDomainObject);
      public Type SourceEntityType => typeof(TSourceEntity);
      public Type TargetEntityType => typeof(TTargetEntity);
      public PropertyInfo DomainProperty { get; }
      public PropertyInfo EntityProperty { get; }

      public ReferenceListRelationEntry(
         string key,
         PropertyInfo domainProperty,
         PropertyInfo entityProperty)
      {
         _relationKey = key;

         DomainProperty = domainProperty;
         EntityProperty = entityProperty;
      }
   }
}
