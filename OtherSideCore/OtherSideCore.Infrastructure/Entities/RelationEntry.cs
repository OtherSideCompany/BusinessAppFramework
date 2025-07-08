using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   internal class RelationEntry<TEntity, TDomainObject> : IRelationEntry where TEntity : IEntity where TDomainObject : DomainObject
   {
      private readonly Func<TDomainObject, Expression<Func<TEntity, bool>>> _predicateBuilder;
      private readonly Action<TEntity, TDomainObject> _relationSetter;
      private readonly Action<TEntity, TDomainObject> _relationDelete;
      private readonly RelationType _relationType;

      public RelationEntry(
         Func<TDomainObject, Expression<Func<TEntity, bool>>> predicateBuilder,
         Action<TEntity, TDomainObject> relationSetter,
         Action<TEntity, TDomainObject> relationDelete,
         RelationType relationType)
      {
         _predicateBuilder = predicateBuilder;
         _relationSetter = relationSetter;
         _relationDelete = relationDelete;
         _relationType = relationType;
      }

      public Expression<Func<IEntity, bool>> GetRelationPredicate(DomainObject domainObject)
      {
         var specificPredicate = _predicateBuilder((TDomainObject)domainObject);

         var parameter = Expression.Parameter(typeof(IEntity), "e");
         var casted = Expression.Convert(parameter, typeof(TEntity));
         var body = Expression.Invoke(specificPredicate, casted);
         return Expression.Lambda<Func<IEntity, bool>>(body, parameter);
      }

      public void SetRelation(IEntity entity, DomainObject domainObject)
      {
         _relationSetter((TEntity)entity, (TDomainObject)domainObject);
      }

      public void DeleteRelation(IEntity entity, DomainObject domainObject)
      {
         _relationDelete((TEntity)entity, (TDomainObject)domainObject);
      }
   }
}
