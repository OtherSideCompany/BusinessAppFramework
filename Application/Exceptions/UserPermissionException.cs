using Domain;

namespace Application.Exceptions
{
   public class UserPermissionException : Exception
   {
      #region Fields



      #endregion

      #region Properties

      public Type TargetType { get; }
      public UserRolePermissionType PermissionType { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserPermissionException(Type targetType, UserRolePermissionType permissionType) : base(BuildMessage(targetType, permissionType))
      {
         TargetType = targetType;
         PermissionType = permissionType;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      private static string BuildMessage(Type targetType, UserRolePermissionType permissionType)
      {
         var objectName = targetType.Name;
         var permission = permissionType switch
         {
            UserRolePermissionType.Create => "create",
            UserRolePermissionType.Read => "read",
            UserRolePermissionType.Update => "update",
            UserRolePermissionType.Delete => "delete",
            _ => "do this action"
         };

         return $"[PERMISSION ERROR] Action \"{permissionType}\" denied on object of type \"{targetType.FullName}\".";
      }

      #endregion
   }
}
