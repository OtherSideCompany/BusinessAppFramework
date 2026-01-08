using OtherSideCore.Application;
using OtherSideCore.Domain;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Relations
{
    public interface IRelationResolver
    {
        bool ContainsParentChildRelationByChildType(Type sourceType, Type relatedType);
        Expression<Func<TEntity, bool>> GetParentChildRelationPredicate<TEntity>(int relatedId, Type relatedType) where TEntity : IEntity;
        void SetParentChildRelation<TEntity>(TEntity entity, Type relatedType, int relatedId) where TEntity : IEntity;

        void DeleteReferenceRelation<TEntity, U>(TEntity entity, int relatedId) where TEntity : IEntity where U : class;
        bool TryGetReferenceRelationEntry(StringKey key, out IReferenceRelationEntry relationEntry);

        bool TryGetReferenceListRelationEntry(StringKey key, out IReferenceListRelationEntry relationEntry);

        IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType);
        IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType);
    }
}
