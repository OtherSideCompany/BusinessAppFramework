using OtherSideCore.Domain;
using System.Linq.Expressions;
using System.Reflection;

namespace OtherSideCore.Application.Relations
{
    public interface IRelationEntry
    {
        StringKey RelationKey { get; }
        RelationType RelationType { get; }
        Type SourceDomainObjectType { get; }
        Type TargetDomainObjectType { get; }
        Type SourceEntityType { get; }
        Type TargetEntityType { get; }
        bool IsSystemManaged { get; }
        bool IsReadOnly { get; }
        PropertyInfo? DomainReferenceProperty { get; }
        Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId);
        void SetRelation(IEntity entity, int? relatedId);
        void DeleteRelation(IEntity entity, int relatedId);
        int? GetRelatedId(IEntity entity);
        string GetDisplayValue(IEntity entity);
    }
}
