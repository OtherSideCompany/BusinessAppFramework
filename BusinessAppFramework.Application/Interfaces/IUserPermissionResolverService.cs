namespace BusinessAppFramework.Application.Interfaces
{
   public interface IUserPermissionResolverService
   {
      Task<bool> CanCreateAsync(string permissionKey, int userId);
      Task<bool> CanUpdateAsync(string permissionKey, int userId);
      Task<bool> CanDeleteAsync(string permissionKey, int userId);
      Task<bool> CanAccessAsync(string permissionKey, int userId);
      Task<bool> CanAccessAllAsync(string[] permissionKeys, int userId);
      Task<bool> CanAccessAnyAsync(string[] permissionKeys, int userId);
      Task<bool> CanReadAsync(string permissionKey, int userId);
   }
}
