using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.Exceptions;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.Attributes;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public static class DomainObjectServiceHelper
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public static async Task<(bool Success, List<T> Result)> TryGetAllAsync<T>(
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         try
         {
            var result = (await service.GetAllAsync()).ToList();
            return (true, result.ToList());
         }
         catch (UserPermissionException e)
         {
            dialogService.Error(BuildPermissionErrorMessage(e.TargetType, e.PermissionType, localizationService));
            return (false, new List<T>());
         }
      }

      public static async Task<(bool, T?)> TryCreateAsync<T>(
         DomainObject? parent,
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         var domainObject = new T();
         var success = await TryCreateAsync<T>(domainObject, parent, service, dialogService, localizationService);
         return (success, success? domainObject : null);
      }

      public static async Task<bool> TryCreateAsync<T>(
         T domainObject,
         DomainObject? parent,         
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         try
         {
            await service.CreateAsync(domainObject, parent);
            return true;
         }
         catch (UserPermissionException e)
         {
            dialogService.Error(BuildPermissionErrorMessage(e.TargetType, e.PermissionType, localizationService));
            return false;
         }
      }

      public static async Task<bool> TryDeleteAsync<T>(
         T domainObject,
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         try
         {
            await service.DeleteAsync(domainObject);
            return true;
         }
         catch (UserPermissionException e)
         {
            dialogService.Error(BuildPermissionErrorMessage(e.TargetType, e.PermissionType, localizationService));
            return false;
         }
      }

      public static async Task<(bool, T?)> TryGetAsync<T>(
         int domainObjectId,
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         try
         {
            return  (true, await service.GetAsync(domainObjectId));
         }
         catch (UserPermissionException e)
         {
            dialogService.Error(BuildPermissionErrorMessage(e.TargetType, e.PermissionType, localizationService));
            return (false, null);
         }
      }

      public static async Task<bool> TrySaveAsync<T>(
         T domainObject,
         IDomainObjectService<T> service,
         IUserDialogService dialogService,
         ILocalizationService localizationService) where T : DomainObject, new()
      {
         try
         {
            await service.SaveAsync(domainObject);
            return true;
         }
         catch (UserPermissionException e)
         {
            dialogService.Error(BuildPermissionErrorMessage(e.TargetType, e.PermissionType, localizationService));
            return false;
         }
      }

      #endregion

      #region Private Methods

      private static string BuildPermissionErrorMessage(Type domainObjectType, UserRolePermissionType userRolePermissionType, ILocalizationService localizationService)
      {
         var permissionKeyAttr = domainObjectType.GetCustomAttributes(typeof(UserRolePermissionKeyAttribute), inherit: false)
                                                 .FirstOrDefault() as UserRolePermissionKeyAttribute;

         var resourceKey = permissionKeyAttr?.Key ?? domainObjectType.Name;
         var objectName = localizationService.GetString(resourceKey) ?? domainObjectType.Name;

         var actionLabel = userRolePermissionType switch
         {
            UserRolePermissionType.Read => "lire",
            UserRolePermissionType.Create => "créer",
            UserRolePermissionType.Update => "modifier",
            UserRolePermissionType.Delete => "supprimer",
            _ => "accéder"
         };

         return $"Vous n'avez pas les droits nécessaires pour {actionLabel} des objets de type {objectName}.";
      }

      #endregion
   }
}
