using OtherSideCore.Domain;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Relations
{
    public interface IParentChildRelationEntry
    {
        StringKey RelationKey { get; }
        Type ChildEntityType { get; }
        Type ParentEntityType { get; }
        Expression<Func<IEntity, bool>> GetRelationPredicate(int parentId);
        void SetRelation(IEntity entity, int? parentId);
    }
}
