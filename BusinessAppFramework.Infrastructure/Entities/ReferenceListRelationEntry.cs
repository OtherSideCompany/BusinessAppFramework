using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BusinessAppFramework.Infrastructure.Entities
{
    public class ReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity> : IInfrastructureReferenceListRelation
        where TSourceDomainObject : DomainObject
        where TTargetDomainObject : DomainObject
        where TSourceEntity : class, IEntity
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

        public IQueryable<int> GetTargetIds(DbContext context, int sourceId)
        {
            var s = Expression.Parameter(typeof(TSourceEntity), "s");
            var nav = Expression.Lambda<Func<TSourceEntity, IEnumerable<TTargetEntity>>>(
                Expression.Property(s, EntityProperty), s);

            return context.Set<TSourceEntity>()
                          .Where(e => e.Id == sourceId)
                          .SelectMany(nav)
                          .Select(t => t.Id);
        }
    }
}
