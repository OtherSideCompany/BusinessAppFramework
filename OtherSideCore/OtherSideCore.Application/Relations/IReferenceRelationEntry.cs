using OtherSideCore.Domain;
using System.Linq.Expressions;
using System.Reflection;

namespace OtherSideCore.Application.Relations
{
    public interface IReferenceRelationEntry
    {
        StringKey RelationKey { get; }
        Type SourceDomainObjectType { get; }
        Type TargetDomainObjectType { get; }
        Type SourceEntityType { get; }
        Type TargetEntityType { get; }
        bool IsSystemManaged { get; }
        PropertyInfo? DomainProperty { get; }
        void SetRelation(IEntity entity, int? relatedId);
        void DeleteRelation(IEntity entity, int relatedId);
        int? GetRelatedId(IEntity entity);
        string GetDisplayValue(IEntity entity);
    }
}
