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

        private readonly List<IRelationEntry> _entries = new();


        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public bool Contains(StringKey key)
        {
            return _entries.Any(r => r.RelationKey.Equals(key));
        }

        public bool ContainsParentChildRelation(Type sourceType, Type relatedType)
        {
            return _entries.Any(r => r.SourceEntityType == sourceType && r.TargetEntityType == relatedType && r.RelationType == RelationType.ParentChild);
        }

        public Expression<Func<TEntity, bool>> GetParentChildRelationPredicate<TEntity>(int relatedId, Type relatedType) where TEntity : IEntity
        {
            var relationEntry = _entries.Where(r => r.SourceEntityType == typeof(TEntity) && r.TargetEntityType == relatedType && r.RelationType == RelationType.ParentChild).FirstOrDefault();

            if (relationEntry == null)
                throw new InvalidOperationException($"No parent child relation entry found for entity type {typeof(TEntity).Name} and relation type {relatedType}");

            var untypedPredicate = relationEntry.GetRelationPredicate(relatedId);

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public void SetParentChildRelation<TEntity>(TEntity entity, Type relatedType, int relatedId, RelationType relationType) where TEntity : IEntity
        {
            var relationEntry = _entries.Where(r => r.SourceEntityType == typeof(TEntity) && r.TargetEntityType == relatedType && r.RelationType == RelationType.ParentChild).FirstOrDefault();

            if (relationEntry == null)
                throw new InvalidOperationException($"No parent child relation entry found for entity type {typeof(TEntity).Name} and relation type {relatedType}");

            relationEntry.SetRelation(entity, relatedId);
        }

        public void DeleteRelation<TEntity, U>(TEntity entity, int relatedId, RelationType relationType) where TEntity : IEntity where U : class
        {
            /*var relationEntry = GetRelationEntry<TEntity>(typeof(U), relationType);
            relationEntry.DeleteRelation(entity, relatedId);*/
        }

        public bool TryGetEntry(StringKey key, out IRelationEntry relationEntry)
        {
            relationEntry = _entries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

        public IEnumerable<IRelationEntry> GetEntriesBySourceType(Type sourceType)
        {
            return _entries.Where(r => r.SourceEntityType == sourceType);
        }

        #endregion

        #region Private Methods

        protected void Register<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           StringKey relationKey,
           Func<int, Expression<Func<TSourceEntity, bool>>> predicateBuilder,
           Action<TSourceEntity, int?> relationSetter,
           Action<TSourceEntity, int> relationDelete,
           Func<TSourceEntity, int?> relatedIdGetter,
           Func<TSourceEntity, TTargetEntity?>? _relatedEntityGetter,
           Func<TTargetEntity, string>? relatedDisplayValueGetter,
           RelationType relationType,
           Expression<Func<TSourceDomainObject, DomainObjectReference?>> domainReferenceExpression,
           bool isSystemManaged,
           bool isReadOnly)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : IEntity
            where TTargetEntity : class, IEntity
        {
            var domainReferenceProperty = ExtractProperty(domainReferenceExpression);
            var entry = new RelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, predicateBuilder, relationSetter, relationDelete, relatedIdGetter, _relatedEntityGetter, relatedDisplayValueGetter, relationType, domainReferenceProperty, isSystemManaged, isReadOnly);

            if (_entries.Any(r => r.RelationKey.Equals(relationKey)))
            {
                throw new ArgumentException($"Cannot add several relation entries with key {relationKey}");
            }
            else if (relationType == RelationType.ParentChild && ContainsParentChildRelation(typeof(TSourceEntity), typeof(TTargetEntity)))
            {
                throw new ArgumentException($"Cannot add several parent child relations entries for types <{typeof(TSourceEntity)},{typeof(TTargetEntity)}>");
            }

            _entries.Add(entry);
        }

        private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReference?>> expr)
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
