using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Entities
{
    public class ReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity> : IReferenceListRelationEntry
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
        public PropertyInfo EntityProperty { get; }

        public ReferenceListRelationEntry(
           StringKey key,
           PropertyInfo domainProperty,
           PropertyInfo entityProperty)
        {
            _relationKey = key;

            DomainProperty = domainProperty;
            EntityProperty = entityProperty;
        }
    }
}
