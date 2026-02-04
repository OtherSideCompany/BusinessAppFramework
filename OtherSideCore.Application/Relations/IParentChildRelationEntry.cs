using OtherSideCore.Domain;
using System.Linq.Expressions;
using System.Reflection;

namespace OtherSideCore.Application.Relations
{
    public interface IParentChildRelationEntry
    {
        StringKey RelationKey { get; }
        Type ChildEntityType { get; }
        Type ParentEntityType { get; }
        PropertyInfo ParentEntityIdProperty { get; }
    }
}
