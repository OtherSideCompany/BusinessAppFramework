using BusinessAppFramework.Domain;
using System.Reflection;

namespace BusinessAppFramework.Application.Relations
{
   public interface IReferenceListRelationEntry
   {
      StringKey RelationKey { get; }
      Type SourceDomainObjectType { get; }
      Type TargetDomainObjectType { get; }
      Type SourceEntityType { get; }
      Type TargetEntityType { get; }
      PropertyInfo DomainProperty { get; }
      PropertyInfo EntityProperty { get; }
   }
}
