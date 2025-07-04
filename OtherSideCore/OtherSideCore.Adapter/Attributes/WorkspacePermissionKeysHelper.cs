using OtherSideCore.Adapter.Views;
using OtherSideCore.Domain.Attributes;
using System.Reflection;

namespace OtherSideCore.Adapter.Attributes
{
   public class WorkspacePermissionKeysHelper
   {
      public static string[] GetPermissionKeys<T>() where T : Workspace
      {
         var attribute = typeof(T).GetCustomAttribute<WorkspacePermissionKeys>();
         return attribute != null ? attribute.Keys : [];
      }

      public static string[] GetPermissionKeys(Type type)
      {
         var attribute = type.GetCustomAttribute<WorkspacePermissionKeys>();
         return attribute != null ? attribute.Keys : [];
      }
   }
}
