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
    public class ReferenceResolver : IReferenceResolver
    {
        #region Fields

        private readonly List<IReferenceRelationEntry> _referenceRelationEntries = new();
        private readonly List<IReferenceListRelationEntry> _referenceListRelationEntries = new();

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public bool TryGetReferenceRelationEntry(string key, out IReferenceRelationEntry relationEntry)
        {
            relationEntry = _referenceRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

        public bool TryGetReferenceListRelationEntry(string key, out IReferenceListRelationEntry relationEntry)
        {
            relationEntry = _referenceListRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

        public IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType)
        {
            for (var t = sourceType; t != null && t != typeof(object); t = t.BaseType)
            {
                foreach (var entry in _referenceRelationEntries.Where(r => r.SourceEntityType == t))
                    yield return entry;
            }
        }

        public IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType)
        {
            for (var t = sourceType; t != null && t != typeof(object); t = t.BaseType)
            {
                foreach (var entry in _referenceListRelationEntries.Where(r => r.SourceEntityType == t))
                    yield return entry;
            }
        }

        public void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           Expression<Func<TSourceDomainObject, DomainObjectReference>> domainExpression,
           Expression<Func<TSourceEntity, int?>> entityIdExpression)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : class, IEntity
            where TTargetEntity : class, IEntity
        {
            var domainProperty = ExtractProperty(domainExpression);

            var tempInstance = Activator.CreateInstance<TSourceDomainObject>();
            var reference = (DomainObjectReference)domainProperty.GetValue(tempInstance)!;
            var relationKey = reference.RelationKey;

            RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainExpression, entityIdExpression);
        }

        public void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           string relationKey,
           Expression<Func<TSourceDomainObject, DomainObjectReference>> domainExpression,
           Expression<Func<TSourceEntity, int?>> entityIdExpression)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : class, IEntity
            where TTargetEntity : class, IEntity
        {
            var domainProperty = ExtractProperty(domainExpression);
            var entityIdProperty = ExtractProperty(entityIdExpression);

            var entry = new ReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainProperty, entityIdProperty);

            if (_referenceRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
            {
                throw new ArgumentException($"Cannot add several reference relation entries with key {relationKey}");
            }

            _referenceRelationEntries.Add(entry);
        }

        public void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
              Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
              Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
               where TSourceDomainObject : DomainObject
               where TTargetDomainObject : DomainObject
               where TSourceEntity : class, IEntity
               where TTargetEntity : class, IEntity
        {
            var domainProperty = ExtractProperty(domainExpression);

            var tempInstance = Activator.CreateInstance<TSourceDomainObject>();
            var reference = (DomainObjectReferenceList)domainProperty.GetValue(tempInstance)!;
            var relationKey = reference.RelationKey;

            RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainExpression, entityExpression);
        }

        public void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
              string relationKey,
              Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
              Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
               where TSourceDomainObject : DomainObject
               where TTargetDomainObject : DomainObject
               where TSourceEntity : class, IEntity
               where TTargetEntity : class, IEntity
        {
            var domainProperty = ExtractProperty(domainExpression);
            var entityProperty = ExtractProperty(entityExpression);

            var entry = new ReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainProperty, entityProperty);

            if (_referenceListRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
            {
                throw new ArgumentException($"Cannot add several reference list relation entries with key {relationKey}");
            }

            _referenceListRelationEntries.Add(entry);
        }

        #endregion

        #region Private Methods

        private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReference>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }

        private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }

        private static PropertyInfo ExtractProperty<TSourceEntity, TTargetEntity>(Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }

        private static PropertyInfo ExtractProperty<TSourceEntity>(Expression<Func<TSourceEntity, int?>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }        

        #endregion
    }
}
