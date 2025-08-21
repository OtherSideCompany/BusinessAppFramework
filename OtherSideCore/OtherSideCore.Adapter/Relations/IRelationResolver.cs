using OtherSideCore.Application;
using OtherSideCore.Domain;
using System.Linq.Expressions;

namespace OtherSideCore.Adapter.Relations
{
   public interface IRelationResolver
   {
      bool Contains(StringKey relationKey);
      bool ContainsParentChildRelation(Type sourceType, Type relatedType);
      Expression<Func<TEntity, bool>> GetParentChildRelationPredicate<TEntity>(int relatedId, Type relatedType) where TEntity : IEntity;
      void SetParentChildRelation<TEntity>(TEntity entity, Type relatedType, int relatedId, RelationType relationType) where TEntity : IEntity;
      void DeleteRelation<TEntity, U>(TEntity entity, int relatedId, RelationType relationType) where TEntity : IEntity where U : class;
      bool TryGetEntry(StringKey key, out IRelationEntry relationEntry);
      IEnumerable<IRelationEntry> GetEntriesBySourceType(Type sourceType);
   }
}
