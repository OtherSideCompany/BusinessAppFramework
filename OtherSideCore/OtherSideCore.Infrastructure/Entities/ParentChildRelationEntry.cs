using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
    public class ParentChildRelationEntry<TSourceEntity, TTargetEntity> : IParentChildRelationEntry
        where TSourceEntity : IEntity
        where TTargetEntity : class, IEntity
    {
        private StringKey _relationKey;
        private readonly Func<int, Expression<Func<TSourceEntity, bool>>> _predicateBuilder;
        private readonly Action<TSourceEntity, int?> _relationSetter;

        public StringKey RelationKey => _relationKey;
        public Type SourceEntityType => typeof(TSourceEntity);
        public Type TargetEntityType => typeof(TTargetEntity);

        public ParentChildRelationEntry(
           StringKey key,
           Func<int, Expression<Func<TSourceEntity, bool>>> predicateBuilder,
            Action<TSourceEntity, int?> relationSetter)
        {
            _relationKey = key;
            _predicateBuilder = predicateBuilder;
            _relationSetter = relationSetter;
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
    }
}
