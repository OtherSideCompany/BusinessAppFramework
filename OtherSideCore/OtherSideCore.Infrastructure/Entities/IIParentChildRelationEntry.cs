using OtherSideCore.Adapter;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Infrastructure.Entities
{
   internal interface IIParentChildRelationEntry
   {
      Expression<Func<IEntity, bool>> GetParentRelationPredicate(DomainObject parent);
      void SetParent(IEntity entity, DomainObject parent);
   }
}
