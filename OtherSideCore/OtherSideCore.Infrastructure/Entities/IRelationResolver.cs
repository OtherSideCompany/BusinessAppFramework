using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public interface IRelationResolver
   {
      bool Contains(Type entityType, Type domainObjectType, RelationType relationType);
      Expression<Func<TEntity, bool>> GetRelationPredicate<TEntity>(int relatedId, RelationType relationType) where TEntity : IEntity;
      Expression<Func<TEntity, bool>> GetRelationPredicate<TEntity>(DomainObject domainObject) where TEntity : IEntity;
      void SetRelation<TEntity>(TEntity entity, DomainObject parent) where TEntity : IEntity;
      void DeleteRelation<TEntity>(TEntity entity, DomainObject parent) where TEntity : IEntity;
      IEnumerable<IRelationEntry> GetEntriesFor<TEntity>() where TEntity : IEntity;
   }
}
