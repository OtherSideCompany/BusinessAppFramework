using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Relations;
using OtherSideCore.Application;
using OtherSideCore.Domain;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   internal class RelationEntry<T, U> : IRelationEntry where T : IEntity where U : class, IEntity
   {
      private StringKey _relationKey;
      private readonly Func<int, Expression<Func<T, bool>>> _predicateBuilder;
      private readonly Action<T, int> _relationSetter;
      private readonly Action<T, int> _relationDelete;
      private readonly Func<T, int?>? _relatedIdGetter;
      private readonly Func<T, U?>? _relatedEntityGetter;
      private readonly RelationType _relationType;
      private bool _isSystemManaged;
      private bool _isReadOnly;

      public StringKey RelationKey => _relationKey;
      public RelationType RelationType => _relationType;
      public Type SourceType => typeof(T);
      public Type RelatedType => typeof(U);
      public bool IsSystemManaged => _isSystemManaged;
      public bool IsReadOnly => _isReadOnly;

      public RelationEntry(
         StringKey key,
         Func<int, Expression<Func<T, bool>>> predicateBuilder,
         Action<T, int> relationSetter,
         Action<T, int> relationDelete,
         Func<T, int?>? relatedIdGetter,
         Func<T, U?>? relatedEntityGetter,
         RelationType relationType,
         bool isSystemManaged,
         bool isReadOnly)
      {
         _relationKey = key;
         _predicateBuilder = predicateBuilder;
         _relationSetter = relationSetter;
         _relationDelete = relationDelete;
         _relatedIdGetter = relatedIdGetter;
         _relatedEntityGetter = relatedEntityGetter;
         _relationType = relationType;
         _isSystemManaged = isSystemManaged;
      }

      public Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId)
      {
         var specificPredicate = _predicateBuilder(relatedId);

         var parameter = Expression.Parameter(typeof(IEntity), "e");
         var casted = Expression.Convert(parameter, typeof(T));
         var body = Expression.Invoke(specificPredicate, casted);
         return Expression.Lambda<Func<IEntity, bool>>(body, parameter);
      }

      public void SetRelation(IEntity entity, int relatedId)
      {
         _relationSetter((T)entity, relatedId);
      }

      public void DeleteRelation(IEntity entity, int relatedId)
      {
         _relationDelete((T)entity, relatedId);
      }

      public int? GetRelatedId(IEntity entity)
      {
         var typedEntity = (T)entity;

         if (_relatedIdGetter != null)
            return _relatedIdGetter(typedEntity);

         if (_relatedEntityGetter != null)
         {
            var related = _relatedEntityGetter(typedEntity);
            return related?.Id;
         }

         throw new InvalidOperationException("No way to get related Id configured.");
      }
   }
}
