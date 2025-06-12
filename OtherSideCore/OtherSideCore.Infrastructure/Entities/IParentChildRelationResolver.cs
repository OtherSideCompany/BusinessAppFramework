using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public interface IParentChildRelationResolver
   {
      bool Contains(Type entityType, Type domainObjectType);
      Expression<Func<TEntity, bool>> GetParentRelationPredicate<TEntity>(DomainObject parent) where TEntity : IEntity;
      void SetParent<TEntity>(TEntity entity, DomainObject parent) where TEntity : IEntity;
   }
}
