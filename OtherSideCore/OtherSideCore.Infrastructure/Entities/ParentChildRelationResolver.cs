using ImageMagick;
using OtherSideCore.Application;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public abstract class ParentChildRelationResolver : IParentChildRelationResolver
   {
      #region Fields

      private readonly Dictionary<(Type EntityType, Type DomainObjectType), IIParentChildRelationEntry> _entries = new();


      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public bool Contains(Type entityType, Type domainObjectType)
      {
        return _entries.ContainsKey((entityType, domainObjectType));
      }

      public Expression<Func<TEntity, bool>> GetParentRelationPredicate<TEntity>(DomainObject parent) where TEntity : IEntity
      {
         var key = (typeof(TEntity), parent.GetType());

         if (!_entries.TryGetValue(key, out var entry))
            throw new InvalidOperationException($"No registered relation for ({typeof(TEntity).Name}, {parent.GetType().Name})");

         var untypedPredicate = entry.GetParentRelationPredicate(parent);

         var parameter = Expression.Parameter(typeof(TEntity), "e");
         var body = Expression.Invoke(untypedPredicate, Expression.Convert(parameter, typeof(IEntity)));
         return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
      }

      public void SetParent<TEntity>(TEntity entity, DomainObject parent) where TEntity : IEntity
      {
         var key = (typeof(TEntity), parent.GetType());

         if (!_entries.TryGetValue(key, out var entry))
            throw new InvalidOperationException($"No registered setter for ({typeof(TEntity).Name}, {parent.GetType().Name})");

         entry.SetParent(entity, parent);
      }

      #endregion

      #region Private Methods

      protected void Register<TEntity, TDomainObject>(
        Func<TDomainObject, Expression<Func<TEntity, bool>>> parentPredicateBuilder,
        Action<TEntity, TDomainObject> parentSetter)
        where TEntity : IEntity
        where TDomainObject : DomainObject
      {
         var key = (typeof(TEntity), typeof(TDomainObject));
         var entry = new ParentChildRelationEntry<TEntity, TDomainObject>(parentPredicateBuilder, parentSetter);
         _entries[key] = entry;
      }

      #endregion
   }
}
