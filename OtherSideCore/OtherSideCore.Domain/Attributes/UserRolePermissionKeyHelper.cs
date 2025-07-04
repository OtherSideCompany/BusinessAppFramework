using OtherSideCore.Domain.DomainObjects;
using System;
using System.Reflection;

namespace OtherSideCore.Domain.Attributes
{
   public static class UserRolePermissionKeyHelper
   {
      public static string GetPermissionKey<T>() where T : DomainObject
      {
         return typeof(T).GetCustomAttribute<UserRolePermissionKeyAttribute>()?.Key;
      }

      public static string GetPermissionKey(Type type)
      {
         return type.GetCustomAttribute<UserRolePermissionKeyAttribute>()?.Key;
      }
   }
}
