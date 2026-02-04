using BusinessAppFramework.Domain;
using System.Reflection;

namespace BusinessAppFramework.Application.Relations
{
   public interface IParentChildRelationEntry
   {
      StringKey RelationKey { get; }
      Type ChildEntityType { get; }
      Type ParentEntityType { get; }
      PropertyInfo ParentEntityIdProperty { get; }
   }
}
