using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
    public class ParentChildRelationEntry<TParentEntity, TChildEntity> : IParentChildRelationEntry
        where TChildEntity : IEntity
        where TParentEntity : class, IEntity
    {
        private StringKey _relationKey;
        private readonly Func<int, Expression<Func<TChildEntity, bool>>> _predicateBuilder;
        private readonly Action<TChildEntity, int?> _relationSetter;

        public StringKey RelationKey => _relationKey;
        public Type ChildEntityType => typeof(TChildEntity);
        public Type ParentEntityType => typeof(TParentEntity);

        public ParentChildRelationEntry(
           StringKey key,
           Func<int, Expression<Func<TChildEntity, bool>>> predicateBuilder,
            Action<TChildEntity, int?> relationSetter)
        {
            _relationKey = key;
            _predicateBuilder = predicateBuilder;
            _relationSetter = relationSetter;
        }

        public Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId)
        {
            var specificPredicate = _predicateBuilder(relatedId);

            var parameter = Expression.Parameter(typeof(IEntity), "e");
            var casted = Expression.Convert(parameter, typeof(TChildEntity));
            var body = Expression.Invoke(specificPredicate, casted);
            return Expression.Lambda<Func<IEntity, bool>>(body, parameter);
        }

        public void SetRelation(IEntity entity, int? relatedId)
        {
            _relationSetter((TChildEntity)entity, relatedId);
        }
    }
}
