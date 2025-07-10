using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Exceptions;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Mail;
using OtherSideCore.Application.Repository;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.Attributes;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;
using System.Threading;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectServiceDependencies _domainObjectServiceDependencies;
      protected readonly IRepository<T> _repository;
      protected readonly IUserContext _userContext;
      protected readonly IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected readonly IUserDialogService _userDialogService;
      protected readonly IDomainObjectFileService _domainObjectFileService;
      protected readonly IPasswordService _passwordService;
      protected readonly IMailService _mailService;
      protected readonly IDomainObjectEventBus _domainObjectEventBus;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectService(
         IRepository<T> repository,
         DomainObjectServiceDependencies domainObjectServiceDependencies)
      {
         _domainObjectServiceDependencies = domainObjectServiceDependencies;
         _repository = repository;
         _userContext = domainObjectServiceDependencies.UserContext;
         _domainObjectServiceFactory = domainObjectServiceDependencies.DomainObjectServiceFactory;
         _userDialogService = domainObjectServiceDependencies.UserDialogService;
         _domainObjectFileService = domainObjectServiceDependencies.DomainObjectFileService;
         _domainObjectEventBus = domainObjectServiceDependencies.DomainObjectEventBus;
         _domainObjectServiceFactory = domainObjectServiceDependencies.DomainObjectServiceFactory;
      }

      #endregion

      #region Public Methods

      public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.ExistsAsync(domainObjectId, cancellationToken);
      }

      public async Task<(bool Success, List<T> Items)> TryGetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default)
      {
         try
         {
            var items = await GetAllAsync(parent, cancellationToken);
            return (true, items);
         }
         catch (UserPermissionException)
         {
            return (false, new List<T>());
         }
      }

      public async Task<List<T>> GetAllAsync(DomainObject? parent = null, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Read))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Read);
         }
         else
         {
            return await _repository.GetAllAsync(parent, cancellationToken);
         }
      }

      public virtual async Task CreateAsync(T domainObject, DomainObject? parent)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Create))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Create);
         }
         else
         {
            await CreateWithoutRightsCheckAsync(domainObject, parent);
         }
      }      

      public virtual async Task<T> CreateAsync(DomainObject? parent)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Create))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Create);
         }
         else
         {
            var domainObject = new T();
            await CreateWithoutRightsCheckAsync(domainObject, parent);
            return domainObject;
         }
      }

      public virtual async Task<bool> DeleteAsync(T domainObject)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Delete))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Delete);
         }
         else
         {
            var deletedDomainObjectId = domainObject.Id;

            if (domainObject is ISystemObject systemObject && !String.IsNullOrEmpty(systemObject.SystemCode))
            {
               _userDialogService.Error("Impossible de supprimer l'objet sélectionné car il est nécéssaire au fonctionnement du système.");
               return false;
            }

            try
            {
               await _domainObjectEventBus.PublishAsync(new DomainObjectDeletingEvent(domainObject));

               await _repository.DeleteAsync(domainObject);

               await _domainObjectEventBus.PublishAsync(new DomainObjectDeletedEvent(domainObject, deletedDomainObjectId));

               try
               {
                  _domainObjectFileService.TryDeleteAssociatedFolder(domainObject);
               }
               catch (IOException)
               {
                  _userDialogService.Error("Impossible de supprimer le dossier associé. Veuillez le supprimer manuellement.\n\n" + _domainObjectFileService.GetAssociatedDirectoryInfo(domainObject).FullName);
               }

               return true;
            }
            catch (Exception)
            {
               _userDialogService.Error("Suppression impossible car des données sont associées");
               return false;
            }
         }
      }

      public async Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Read))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Read);
         }
         else
         {
            return await _repository.GetAsync(domainObjectId, cancellationToken);
         }
      }

      public async Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Read))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Read);
         }
         else
         {
            return await _repository.GetFromSystemCodeAsync(systemCode, cancellationToken);
         }
      }

      public virtual async Task SaveAsync(T domainObject)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Update))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Update);
         }
         else
         {
            if (domainObject is ISystemObject systemObject && !String.IsNullOrEmpty(systemObject.SystemCode))
            {
               _userDialogService.Error("Impossible de modifier l'utilisateur car il est nécéssaire au fonctionnement du système.");
               return;
            }

            await _repository.SaveAsync(domainObject, _userContext.Id, _userContext.GetName());
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(domainObject));
         }
      }

      public async Task SaveIndexAsync(IIndexable domainObject)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Update))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Update);
         }
         else
         {
            await _repository.SaveIndexAsync(domainObject, _userContext.Id, _userContext.GetName());
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent((DomainObject)domainObject));
         }
      }

      public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Read))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Read);
         }
         else
         {
            return await _repository.GetDomainObjectReferencesAsync(relationKey, domainObjectId, cancellationToken);
         }
      }

      public async Task<DomainObjectReference> CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Update))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Update);
         }
         else
         {
            return await _repository.CreateDomainObjectReferenceAsync(relationKey, domainObjectId, domainObjectReferenceId, referenceType, cancellationToken);
         }
      }

      public async Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Delete))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Update);
         }
         else
         {
            await _repository.DeleteDomainObjectReferenceAsync(relationKey, domainObjectId, domainObjectReference, cancellationToken);
         }
      }

      public virtual async Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken = default)
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Update))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Update);
         }
         else
         {
            await _repository.SetParentAsync(domainObject, parent, cancellationToken);
         }
      }

      public virtual async Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject
      {
         if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, UserRolePermissionType.Read))
         {
            throw new UserPermissionException(typeof(T), UserRolePermissionType.Read);
         }
         else
         {
            return await _repository.GetParentIdAsync<U>(domainObject, cancellationToken);
         }
      }

      #endregion

      #region Private Methods

      private async Task CreateWithoutRightsCheckAsync(T domainObject, DomainObject? parent)
      {
         if (domainObject is IIndexable indexableDomainObject && parent != null)
         {
            indexableDomainObject.Index = ((IIndexableRepository)_repository).GetNewIndex(parent);
         }

         await _repository.CreateAsync(domainObject, parent, _userContext.Id, _userContext.GetName());
         await _domainObjectEventBus.PublishAsync(new DomainObjectCreatedEvent(domainObject));
      }

      protected async Task<bool> CheckRightAsync(string permissionKey, int userId, UserRolePermissionType userRolePermissionType)
      {
         if (userRolePermissionType == UserRolePermissionType.Create)
         {
            return await _domainObjectServiceDependencies.UserPermissionResolverService.CanCreateAsync(permissionKey, userId);
         }
         else if (userRolePermissionType == UserRolePermissionType.Access)
         {
            return await _domainObjectServiceDependencies.UserPermissionResolverService.CanAccessAsync(permissionKey, userId);
         }
         else if (userRolePermissionType == UserRolePermissionType.Read)
         {
            return await _domainObjectServiceDependencies.UserPermissionResolverService.CanReadAsync(permissionKey, userId);
         }
         else if (userRolePermissionType == UserRolePermissionType.Delete)
         {
            return await _domainObjectServiceDependencies.UserPermissionResolverService.CanDeleteAsync(permissionKey, userId);
         }
         else if (userRolePermissionType == UserRolePermissionType.Update)
         {
            return await _domainObjectServiceDependencies.UserPermissionResolverService.CanUpdateAsync(permissionKey, userId);
         }
         else
         {
            return false;
         }

      }

      #endregion
   }
}
