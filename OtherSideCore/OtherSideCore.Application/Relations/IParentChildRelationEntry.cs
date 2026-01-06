using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OtherSideCore.Application.Relations
{
    public interface IParentChildRelationEntry
    {
        StringKey RelationKey { get; }
        Type SourceEntityType { get; }
        Type TargetEntityType { get; }
        Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId);
        void SetRelation(IEntity entity, int? relatedId);
    }
}
