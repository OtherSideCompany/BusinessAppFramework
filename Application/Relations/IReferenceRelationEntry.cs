using Domain;
using System.Reflection;

namespace Application.Relations
{
   public interface IReferenceRelationEntry
   {
      StringKey RelationKey { get; }
      Type SourceDomainObjectType { get; }
      Type TargetDomainObjectType { get; }
      Type SourceEntityType { get; }
      Type TargetEntityType { get; }
      PropertyInfo DomainProperty { get; }
      PropertyInfo EntityIdProperty { get; }
   }
}
