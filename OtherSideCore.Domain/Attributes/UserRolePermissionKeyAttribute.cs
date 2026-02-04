using System;

namespace OtherSideCore.Domain.Attributes
{
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   public sealed class UserRolePermissionKeyAttribute : Attribute
   {
      public string Key { get; }

      public UserRolePermissionKeyAttribute(string key)
      {
         Key = key;
      }
   }
}
