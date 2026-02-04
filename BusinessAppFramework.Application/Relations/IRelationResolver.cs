using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Relations
{
   public interface IRelationResolver
   {
      bool ContainsParentChildRelationByChildType(Type sourceType, Type relatedType);
      bool TryGetReferenceRelationEntry(StringKey key, out IReferenceRelationEntry relationEntry);
      bool TryGetReferenceListRelationEntry(StringKey key, out IReferenceListRelationEntry relationEntry);
      bool TryGetParentChildRelationEntry(StringKey key, out IParentChildRelationEntry relationEntry);
      IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType);
      IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType);
   }
}
