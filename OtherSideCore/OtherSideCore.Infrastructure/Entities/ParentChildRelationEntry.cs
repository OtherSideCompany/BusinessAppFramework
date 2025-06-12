using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   internal class ParentChildRelationEntry<TEntity, TDomainObject> : IIParentChildRelationEntry where TEntity : IEntity where TDomainObject : DomainObject
   {
      private readonly Func<TDomainObject, Expression<Func<TEntity, bool>>> _parentPredicateBuilder;
      private readonly Action<TEntity, TDomainObject> _parentSetter;

      public ParentChildRelationEntry(
         Func<TDomainObject, Expression<Func<TEntity, bool>>> parentPredicateBuilder,
         Action<TEntity, TDomainObject> parentSetter)
      {
         _parentPredicateBuilder = parentPredicateBuilder;
         _parentSetter = parentSetter;
      }

      public Expression<Func<IEntity, bool>> GetParentRelationPredicate(DomainObject parent)
      {
         var specificPredicate = _parentPredicateBuilder((TDomainObject)parent);

         var parameter = Expression.Parameter(typeof(IEntity), "e");
         var casted = Expression.Convert(parameter, typeof(TEntity));
         var body = Expression.Invoke(specificPredicate, casted);
         return Expression.Lambda<Func<IEntity, bool>>(body, parameter);
      }

      public void SetParent(IEntity entity, DomainObject parent)
      {
         _parentSetter((TEntity)entity, (TDomainObject)parent);
      }
   }
}
