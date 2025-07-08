using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public abstract class RelationResolver : IRelationResolver
   {
      #region Fields

      private readonly Dictionary<(Type EntityType, Type DomainObjectType), IRelationEntry> _entries = new();


      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public bool Contains(Type entityType, Type domainObjectType, RelationType relationType)
      {
        return _entries.ContainsKey((entityType, domainObjectType)) && _entries[(entityType, domainObjectType)].RelationType == relationType;
      }

      public Expression<Func<TEntity, bool>> GetRelationPredicate<TEntity>(int relatedId, RelationType relationType) where TEntity : IEntity
      {
         var keyMatches = _entries.Where(e => e.Key.EntityType == typeof(TEntity) && e.Value.RelationType == relationType)
                                  .Select(e => e.Value)
                                  .FirstOrDefault();

         if (keyMatches == null)
            throw new InvalidOperationException($"No relation entry found for entity type {typeof(TEntity).Name} and relation type {relationType}");

         // Obtenir l'expression sur IEntity
         var untypedPredicate = keyMatches.GetRelationPredicate(relatedId);

         // Adapter à TEntity
         var parameter = Expression.Parameter(typeof(TEntity), "e");
         var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
         return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
      }

      public Expression<Func<TEntity, bool>> GetRelationPredicate<TEntity>(DomainObject domainObject) where TEntity : IEntity
      {
         var key = (typeof(TEntity), domainObject.GetType());

         if (!_entries.TryGetValue(key, out var entry))
            throw new InvalidOperationException($"No registered relation for ({typeof(TEntity).Name}, {domainObject.GetType().Name})");

         var untypedPredicate = entry.GetRelationPredicate(domainObject);

         var parameter = Expression.Parameter(typeof(TEntity), "e");
         var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
         return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
      }

      public void SetRelation<TEntity>(TEntity entity, DomainObject domainObject) where TEntity : IEntity
      {
         var key = (typeof(TEntity), domainObject.GetType());

         if (!_entries.TryGetValue(key, out var entry))
            throw new InvalidOperationException($"No registered setter for ({typeof(TEntity).Name}, {domainObject.GetType().Name})");

         entry.SetRelation(entity, domainObject);
      }

      public void DeleteRelation<TEntity>(TEntity entity, DomainObject domainObject) where TEntity : IEntity
      {
         var key = (typeof(TEntity), domainObject.GetType());

         if (!_entries.TryGetValue(key, out var entry))
            throw new InvalidOperationException($"No registered delete for ({typeof(TEntity).Name}, {domainObject.GetType().Name})");

         entry.DeleteRelation(entity, domainObject);
      }

      public IEnumerable<IRelationEntry> GetEntriesFor<TEntity>() where TEntity : IEntity
      {
         var entityType = typeof(TEntity);
         return _entries.Where(e => e.Key.EntityType == entityType).Select(e => e.Value);
      }

      #endregion

      #region Private Methods

      protected void Register<TEntity, TDomainObject>(
        Func<TDomainObject, Expression<Func<TEntity, bool>>> predicateBuilder,
        Action<TEntity, TDomainObject> relationSetter,
        Action<TEntity, TDomainObject> relationDelete,
        RelationType relationType)
        where TEntity : IEntity
        where TDomainObject : DomainObject
      {
         var key = (typeof(TEntity), typeof(TDomainObject));
         var entry = new RelationEntry<TEntity, TDomainObject>(predicateBuilder, relationSetter, relationDelete, relationType);
         _entries[key] = entry;
      }

      #endregion
   }
}
