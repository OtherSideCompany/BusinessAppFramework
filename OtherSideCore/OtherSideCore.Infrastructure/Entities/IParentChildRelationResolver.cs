using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public interface IParentChildRelationResolver
   {
      Expression<Func<TEntity, bool>> GetParentRelationPredicate<TEntity>(DomainObject parent) where TEntity : IEntity;
      void SetParent<TEntity>(TEntity entity, DomainObject parent) where TEntity : IEntity;
   }
}
