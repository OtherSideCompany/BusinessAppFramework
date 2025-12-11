using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Entities
{
    internal class RelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity> : IRelationEntry
        where TSourceDomainObject : DomainObject
        where TTargetDomainObject : DomainObject
        where TSourceEntity : IEntity 
        where TTargetEntity : class, IEntity
    {
        private StringKey _relationKey;
        private readonly Func<int, Expression<Func<TSourceEntity, bool>>> _predicateBuilder;
        private readonly Action<TSourceEntity, int?> _relationSetter;
        private readonly Action<TSourceEntity, int> _relationDelete;
        private readonly Func<TSourceEntity, int?>? _relatedIdGetter;
        private readonly Func<TSourceEntity, TTargetEntity?>? _relatedEntityGetter;
        private readonly Func<TTargetEntity, string>? _relatedDisplayValueGetter;
        private readonly RelationType _relationType;
        private bool _isSystemManaged;
        private bool _isReadOnly;

        public StringKey RelationKey => _relationKey;
        public RelationType RelationType => _relationType;
        public Type SourceDomainObjectType => typeof(TSourceDomainObject);
        public Type TargetDomainObjectType => typeof(TTargetDomainObject);
        public Type SourceEntityType => typeof(TSourceEntity);
        public Type TargetEntityType => typeof(TTargetEntity);
        public bool IsSystemManaged => _isSystemManaged;
        public bool IsReadOnly => _isReadOnly;
        public PropertyInfo? DomainReferenceProperty { get; }

        public RelationEntry(
           StringKey key,
           Func<int, Expression<Func<TSourceEntity, bool>>> predicateBuilder,
           Action<TSourceEntity, int?> relationSetter,
           Action<TSourceEntity, int> relationDelete,
           Func<TSourceEntity, int?>? relatedIdGetter,
           Func<TSourceEntity, TTargetEntity?>? relatedEntityGetter,
           Func<TTargetEntity, string>? relatedDisplayValueGetter,
           RelationType relationType,
           PropertyInfo? domainReferenceProperty,
           bool isSystemManaged,
           bool isReadOnly)
        {
            _relationKey = key;
            _predicateBuilder = predicateBuilder;
            _relationSetter = relationSetter;
            _relationDelete = relationDelete;
            _relatedIdGetter = relatedIdGetter;
            _relatedEntityGetter = relatedEntityGetter;
            _relatedDisplayValueGetter = relatedDisplayValueGetter;
            _relationType = relationType;
            _isSystemManaged = isSystemManaged;

            DomainReferenceProperty = domainReferenceProperty;
        }

        public Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId)
        {
            var specificPredicate = _predicateBuilder(relatedId);

            var parameter = Expression.Parameter(typeof(IEntity), "e");
            var casted = Expression.Convert(parameter, typeof(TSourceEntity));
            var body = Expression.Invoke(specificPredicate, casted);
            return Expression.Lambda<Func<IEntity, bool>>(body, parameter);
        }

        public void SetRelation(IEntity entity, int? relatedId)
        {
            _relationSetter((TSourceEntity)entity, relatedId);
        }

        public void DeleteRelation(IEntity entity, int relatedId)
        {
            _relationDelete((TSourceEntity)entity, relatedId);
        }

        public int? GetRelatedId(IEntity entity)
        {
            var typedEntity = (TSourceEntity)entity;

            if (_relatedIdGetter != null)
                return _relatedIdGetter(typedEntity);

            if (_relatedEntityGetter != null)
            {
                var related = _relatedEntityGetter(typedEntity);
                return related?.Id;
            }

            throw new InvalidOperationException("No way to get related Id configured.");
        }

        public string GetDisplayValue(IEntity entity)
        {
            var displayValue = String.Empty;

            var typedEntity = (TTargetEntity)entity;

            if (_relatedDisplayValueGetter != null)
                displayValue = _relatedDisplayValueGetter(typedEntity);

            return displayValue;
        }
    }
}
