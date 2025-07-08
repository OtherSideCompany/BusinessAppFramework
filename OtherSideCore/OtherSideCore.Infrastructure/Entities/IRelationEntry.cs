using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   public interface IRelationEntry
   {
      RelationType RelationType { get; }
      Expression<Func<IEntity, bool>> GetRelationPredicate(DomainObject domainObject);
      void SetRelation(IEntity entity, DomainObject domainObject);
      void DeleteRelation(IEntity entity, DomainObject domainObject);
   }
}
