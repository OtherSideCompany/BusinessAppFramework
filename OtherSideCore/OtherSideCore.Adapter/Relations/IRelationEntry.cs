using OtherSideCore.Adapter;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Linq.Expressions;

namespace OtherSideCore.Adapter.Relations
{
   public interface IRelationEntry
   {
      StringKey RelationKey { get; }
      RelationType RelationType { get; }
      Type SourceType { get; }
      Type RelatedType { get; }
      bool IsSystemManaged { get; }
      bool IsReadOnly { get; }      
      Expression<Func<IEntity, bool>> GetRelationPredicate(int relatedId);
      void SetRelation(IEntity entity, int relatedId);
      void DeleteRelation(IEntity entity, int relatedId);
      int? GetRelatedId(IEntity entity);
   }
}
