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

namespace OtherSideCore.Application.Services
{
    public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
    {
        #region Fields

        protected DomainObjectServiceDependencies _domainObjectServiceDependencies;
        protected readonly IRepository<T> _repository;
        protected readonly IDomainObjectServiceFactory _domainObjectServiceFactory;
        protected readonly IDomainObjectFileService _domainObjectFileService;
        protected readonly IPasswordService _passwordService;
        protected readonly IMailService _mailService;
        protected readonly IDomainObjectEventBus _domainObjectEventBus;
        protected readonly IUserContext _userContext;

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
            _domainObjectServiceFactory = domainObjectServiceDependencies.DomainObjectServiceFactory;
            _domainObjectFileService = domainObjectServiceDependencies.DomainObjectFileService;
            _domainObjectEventBus = domainObjectServiceDependencies.DomainObjectEventBus;
            _domainObjectServiceFactory = domainObjectServiceDependencies.DomainObjectServiceFactory;
            _userContext = domainObjectServiceDependencies.UserContext;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _repository.ExistsAsync(domainObjectId, cancellationToken));
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
            return await WithReadPermissionAsync(() => _repository.GetAllAsync(parent, cancellationToken));
        }

        public virtual async Task CreateAsync(T domainObject, DomainObject? parent)
        {
            await WithCreationPermissionAsync(() => CreateWithoutRightsCheckAsync(domainObject, parent));
        }

        public virtual async Task<T> CreateAsync(DomainObject? parent)
        {
            var domainObject = new T();
            await WithCreationPermissionAsync(() => CreateWithoutRightsCheckAsync(domainObject, parent));
            return domainObject;

        }

        public virtual async Task<bool> DeleteAsync(T domainObject)
        {
            var deletedDomainObjectId = domainObject.Id;

            if (domainObject is ISystemObject systemObject && !String.IsNullOrEmpty(systemObject.SystemCode))
            {
                throw new InvalidOperationException("Impossible de supprimer l'objet sélectionné car il est nécéssaire au fonctionnement du système.");
            }

            try
            {
                await _domainObjectEventBus.PublishAsync(new DomainObjectDeletingEvent(domainObject));

                await WithDeletePermissionAsync(() => _repository.DeleteAsync(domainObject));

                domainObject.Id = 0;
                domainObject.LastModifiedById = null;
                domainObject.LastModifiedByName = null;
                domainObject.CreatedById = null;
                domainObject.CreatedByName = null;
                domainObject.CreationDate = default;
                domainObject.LastModifiedDateTime = default;

                await _domainObjectEventBus.PublishAsync(new DomainObjectDeletedEvent(domainObject, deletedDomainObjectId));

                _domainObjectFileService.TryDeleteAssociatedFolder(domainObject);

                return true;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Suppression impossible car des données sont associées");
            }

        }

        public async Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _repository.GetAsync(domainObjectId, cancellationToken));
        }

        public async Task<T> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _repository.GetFromSystemCodeAsync(systemCode, cancellationToken));
        }

        public virtual async Task SaveAsync(T domainObject)
        {
            if (domainObject is ISystemObject systemObject && !String.IsNullOrEmpty(systemObject.SystemCode))
            {
                throw new SystemObjectModificationException();
            }

            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.LastModifiedById = _userContext.Id;
            domainObject.LastModifiedByName = _userContext.GetName();

            await WithUpdatePermissionAsync(() => _repository.SaveAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(domainObject));
        }

        public async Task SaveIndexAsync(IIndexable domainObject)
        {
            ((DomainObject)domainObject).LastModifiedDateTime = DateTime.Now;
            ((DomainObject)domainObject).LastModifiedById = _userContext.Id;
            ((DomainObject)domainObject).LastModifiedByName = _userContext.GetName();

            await WithUpdatePermissionAsync(() => _repository.SaveIndexAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent((DomainObject)domainObject));
        }

        public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(StringKey relationKey, int domainObjectId, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _repository.GetDomainObjectReferencesAsync(relationKey, domainObjectId, cancellationToken));
        }

        public async Task CreateDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, int domainObjectReferenceId, CancellationToken cancellationToken = default)
        {
            await WithUpdateDomainObjectReferencePermissionAsync(() => _repository.CreateDomainObjectReferenceAsync(relationKey, domainObjectId, domainObjectReferenceId, cancellationToken));
        }

        public async Task DeleteDomainObjectReferenceAsync(StringKey relationKey, int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default)
        {
            await WithUpdatePermissionAsync(() => _repository.DeleteDomainObjectReferenceAsync(relationKey, domainObjectId, domainObjectReference, cancellationToken));
        }

        public virtual async Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken = default)
        {
            await WithUpdatePermissionAsync(() => _repository.SetParentAsync(domainObject, parent, cancellationToken));
        }

        public virtual async Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject
        {
            return await WithReadPermissionAsync(() => _repository.GetParentIdAsync<U>(domainObject, cancellationToken));
        }

        #endregion

        #region Private Methods

        private async Task CreateWithoutRightsCheckAsync(T domainObject, DomainObject? parent)
        {
            if (domainObject is IIndexable indexableDomainObject && parent != null)
            {
                indexableDomainObject.Index = ((IIndexableRepository)_repository).GetNewIndex(parent);
            }

            domainObject.CreationDate = DateTime.Now;
            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.CreatedById = _userContext.Id;
            domainObject.CreatedByName = _userContext.GetName();
            domainObject.LastModifiedById = _userContext.Id;
            domainObject.LastModifiedByName = _userContext.GetName();

            await _repository.CreateAsync(domainObject, parent);
            await _domainObjectEventBus.PublishAsync(new DomainObjectCreatedEvent(domainObject));
        }

        private async Task<bool> CheckRightAsync(string permissionKey, int userId, UserRolePermissionType userRolePermissionType)
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

        private async Task<U> WithPermissionAsync<U>(UserRolePermissionType permissionType, Func<Task<U>> action)
        {
            if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, permissionType))
                throw new UserPermissionException(typeof(T), permissionType);

            return await action();
        }

        private async Task WithPermissionAsync(UserRolePermissionType permissionType, Func<Task> action)
        {
            if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _userContext.Id, permissionType))
                throw new UserPermissionException(typeof(T), permissionType);

            await action();
        }

        protected Task WithCreationPermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Create, action);
        protected Task<T> WithCreationPermissionAsync(Func<Task<T>> action) => WithPermissionAsync(UserRolePermissionType.Create, action);
        protected Task<U> WithReadPermissionAsync<U>(Func<Task<U>> action) => WithPermissionAsync<U>(UserRolePermissionType.Read, action);
        protected Task WithUpdatePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Update, action);
        protected Task WithUpdateDomainObjectReferencePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Update, action);
        protected Task WithDeletePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Delete, action);

        #endregion
    }
}
