using Domain;
using System.Reflection;

namespace Application.Relations
{
   public interface IParentChildRelationEntry
   {
      StringKey RelationKey { get; }
      Type ChildEntityType { get; }
      Type ParentEntityType { get; }
      PropertyInfo ParentEntityIdProperty { get; }
   }
}
