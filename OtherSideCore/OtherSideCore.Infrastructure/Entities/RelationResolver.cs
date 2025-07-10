using ImageMagick;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Relations;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
         return _entries.Any(r => r.SourceType == sourceType && r.RelatedType == relatedType && r.RelationType == RelationType.ParentChild);
      }

      public Expression<Func<TEntity, bool>> GetParentChildRelationPredicate<TEntity>(int relatedId, Type relatedType) where TEntity : IEntity
      {
         var relationEntry = _entries.Where(r => r.SourceType == typeof(TEntity) && r.RelatedType == relatedType && r.RelationType == RelationType.ParentChild).FirstOrDefault();

         if (relationEntry == null)
            throw new InvalidOperationException($"No parent child relation entry found for entity type {typeof(TEntity).Name} and relation type {relatedType}");

         var untypedPredicate = relationEntry.GetRelationPredicate(relatedId);

         var parameter = Expression.Parameter(typeof(TEntity), "e");
         var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
         return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
      }

      public void SetRelation<TEntity>(TEntity entity, Type relatedType, int relatedId, RelationType relationType) where TEntity : IEntity
      {
         /*var relationEntry = GetRelationEntry<TEntity>(relatedType, relationType);
         relationEntry.SetRelation(entity, relatedId);*/
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
         return _entries.Where(r => r.SourceType == sourceType);
      }

      #endregion

      #region Private Methods

      protected void Register<T, U>(
         StringKey relationKey,
         Func<int, Expression<Func<T, bool>>> predicateBuilder,
         Action<T, int> relationSetter,
         Action<T, int> relationDelete,
         Func<T, int?> relatedIdGetter,
         Func<T, U?>? _relatedEntityGetter,
         RelationType relationType,
         bool isSystemManaged,
         bool isReadOnly)
        where T : IEntity
        where U : class, IEntity
      {
         var entry = new RelationEntry<T, U>(relationKey, predicateBuilder, relationSetter, relationDelete, relatedIdGetter, _relatedEntityGetter, relationType, isSystemManaged, isReadOnly);

         if (_entries.Any(r => r.RelationKey.Equals(relationKey)))
         {
            throw new ArgumentException($"Cannot add several relation entries with key {relationKey}");
         }
         else if (relationType == RelationType.ParentChild && ContainsParentChildRelation(typeof(T), typeof(U)))
         {
            throw new ArgumentException($"Cannot add several parent child relations entries for types <{typeof(T)},{typeof(U)}>");
         }

         _entries.Add(entry);
      }

      #endregion
   }
}
