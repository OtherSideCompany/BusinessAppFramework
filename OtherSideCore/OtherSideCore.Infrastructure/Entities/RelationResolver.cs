using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Entities
{
    public abstract class RelationResolver : IRelationResolver
    {
        #region Fields

        private readonly List<IReferenceRelationEntry> _referenceRelationEntries = new();
        private readonly List<IReferenceListRelationEntry> _referenceListRelationEntries = new();
        private readonly List<IParentChildRelationEntry> _parentChildRelationEntries = new();

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public bool ContainsParentChildRelationBySourceType(Type sourceType, Type relatedType)
        {
            return _parentChildRelationEntries.Any(r => r.SourceEntityType == sourceType && r.TargetEntityType == relatedType);
        }

        public Expression<Func<TEntity, bool>> GetParentChildRelationPredicate<TEntity>(int relatedId, Type relatedType) where TEntity : IEntity
        {
            var relationEntry = _parentChildRelationEntries.Where(r => r.SourceEntityType == typeof(TEntity) && r.TargetEntityType == relatedType).FirstOrDefault();

            if (relationEntry == null)
                throw new InvalidOperationException($"No parent child relation entry found for entity type {typeof(TEntity).Name} and relation type {relatedType}");

            var untypedPredicate = relationEntry.GetRelationPredicate(relatedId);

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public void SetParentChildRelation<TEntity>(TEntity entity, Type relatedType, int relatedId) where TEntity : IEntity
        {
            var relationEntry = _parentChildRelationEntries.Where(r => r.SourceEntityType == typeof(TEntity) && r.TargetEntityType == relatedType).FirstOrDefault();

            if (relationEntry == null)
                throw new InvalidOperationException($"No parent child relation entry found for entity type {typeof(TEntity).Name} and relation type {relatedType}");

            relationEntry.SetRelation(entity, relatedId);
        }

        public void DeleteReferenceRelation<TEntity, U>(TEntity entity, int relatedId) where TEntity : IEntity where U : class
        {
            /*var relationEntry = GetRelationEntry<TEntity>(typeof(U), relationType);
            relationEntry.DeleteRelation(entity, relatedId);*/
        }

        public bool TryGetReferenceRelationEntry(StringKey key, out IReferenceRelationEntry relationEntry)
        {
            relationEntry = _referenceRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

        public bool TryGetReferenceListRelationEntry(StringKey key, out IReferenceListRelationEntry relationEntry)
        {
            relationEntry = _referenceListRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

        public IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType)
        {
            return _referenceRelationEntries.Where(r => r.SourceEntityType == sourceType);
        }

        public IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType)
        {
            return _referenceListRelationEntries.Where(r => r.SourceEntityType == sourceType);
        }

        #endregion

        #region Private Methods

        private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReference?>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }

        private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReferenceList?>> expr)
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

        protected void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           StringKey relationKey,
           Action<TSourceEntity, int?> relationSetter,
           Action<TSourceEntity, int> relationDelete,
           Func<TSourceEntity, int?> relatedIdGetter,
           Func<TSourceEntity, TTargetEntity?>? _relatedEntityGetter,
           Func<TTargetEntity, string>? relatedDisplayValueGetter,
           Expression<Func<TSourceDomainObject, DomainObjectReference?>> domainExpression,
           bool isSystemManaged,
           bool isReadOnly)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : IEntity
            where TTargetEntity : class, IEntity
        {
            var domainProperty = ExtractProperty(domainExpression);
            var entry = new ReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, relationSetter, relationDelete, relatedIdGetter, _relatedEntityGetter, relatedDisplayValueGetter, domainProperty, isSystemManaged);

            if (_referenceRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
            {
                throw new ArgumentException($"Cannot add several reference relation entries with key {relationKey}");
            }

            _referenceRelationEntries.Add(entry);
        }    

        protected void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
              StringKey relationKey,             
              Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
              Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
               where TSourceDomainObject : DomainObject
               where TTargetDomainObject : DomainObject
               where TSourceEntity : IEntity
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

        protected void RegisterParentChildRelationEntry<TSourceEntity, TTargetEntity>(
              StringKey relationKey,
              Func<int, Expression<Func<TSourceEntity, bool>>> predicateBuilder,
              Action<TSourceEntity, int?> relationSetter)
               where TSourceEntity : IEntity
               where TTargetEntity : class, IEntity
        {
            var entry = new ParentChildRelationEntry<TSourceEntity, TTargetEntity>(relationKey, predicateBuilder, relationSetter);

            if (_parentChildRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
            {
                throw new ArgumentException($"Cannot add several relation entries with key {relationKey}");
            }
            else if (ContainsParentChildRelationBySourceType(typeof(TSourceEntity), typeof(TTargetEntity)))
            {
                throw new ArgumentException($"Cannot add several parent child relations entries for types <{typeof(TSourceEntity)},{typeof(TTargetEntity)}>");
            }

            _parentChildRelationEntries.Add(entry);
        }

        #endregion
    }
}
