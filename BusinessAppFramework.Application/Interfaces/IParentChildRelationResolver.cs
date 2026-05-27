using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IParentChildRelationResolver
    {
        bool ContainsParentChildRelationByChildType(Type sourceType, Type relatedType);
        bool TryGetParentChildRelationEntry(StringKey key, out IParentChildRelationEntry relationEntry);
    }
}
