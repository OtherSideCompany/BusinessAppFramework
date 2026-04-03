using BusinessAppFramework.Application.DomainObjectEvents;
using BusinessAppFramework.Application.Exceptions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Mail;
using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.Attributes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.Domain.Services;

namespace BusinessAppFramework.Application.Services
{
    public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
    {
        #region Fields

        protected DomainObjectServiceDependencies _domainObjectServiceDependencies;
        protected readonly IRepository<T> _repository;
        protected readonly IPasswordService _passwordService;
        protected readonly IMailService _mailService;
        protected readonly IDomainObjectEventBus _domainObjectEventBus;
        protected readonly ICurrentUserService _currentUserService;

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
            _domainObjectEventBus = domainObjectServiceDependencies.DomainObjectEventBus;
            _currentUserService = domainObjectServiceDependencies.CurrentUserService;
            _passwordService = domainObjectServiceDependencies.PasswordService;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _repository.ExistsAsync(domainObjectId, cancellationToken));
        }

        public virtual async Task CreateAsync(T domainObject)
        {
            await WithCreationPermissionAsync(() => CreateWithoutRightsCheckAsync(domainObject));
            await LoadReferencesAsync(domainObject);
        }

        public virtual async Task<T> CreateAsync()
        {
            var domainObject = new T();
            await CreateAsync(domainObject);
            return domainObject;
        }

        public virtual async Task<bool> DeleteAsync(int domainObjectId)
        {
            if (await IsSystemObjectAsync(domainObjectId))
            {
                throw new InvalidOperationException("Impossible de supprimer l'objet sélectionné car il est nécéssaire au fonctionnement du système.");
            }

            try
            {
                await _domainObjectEventBus.PublishAsync(new DomainObjectDeletingEvent(typeof(T), domainObjectId));

                await WithDeletePermissionAsync(() => _repository.DeleteAsync(domainObjectId));

                await _domainObjectEventBus.PublishAsync(new DomainObjectDeletedEvent(typeof(T), domainObjectId));

                return true;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Suppression impossible car des données sont associées");
            }
        }

        public virtual async Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var domainObject = await WithReadPermissionAsync(() => _repository.GetAsync(domainObjectId, cancellationToken));

            if (domainObject != null)
            {
                await LoadReferencesAsync(domainObject);
            }

            return domainObject;
        }

        public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var domainObjects = await WithReadPermissionAsync(() => _repository.GetAllAsync(cancellationToken));

            domainObjects.ForEach(async domainObject => await LoadReferencesAsync(domainObject));

            return domainObjects;
        }

        public virtual async Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var domainObject = await GetAsync(domainObjectId, cancellationToken);

            if (domainObject != null)
            {
                await _domainObjectServiceDependencies.ReferenceHydrator.HydrateAsync(domainObject);
            }

            return domainObject;
        }

        public virtual async Task<DomainObjectReference> GetHydratedDomainObjectReference(int domainObjectId, string relationKey)
        {
            var domainObjectReference = new DomainObjectReference
            {
                DomainObjectId = domainObjectId,
                RelationKey = relationKey
            };

            await _domainObjectServiceDependencies.RelationService.HydrateDomainObjectReferenceAsync(domainObjectReference);

            return domainObjectReference;
        }

        public async Task<DomainObjectReferenceListItem> GetHydratedDomainObjectReferenceListItem(int domainObjectId, string relationKey)
        {
            var domainObjectReferenceListItem = new DomainObjectReferenceListItem
            {
                DomainObjectId = domainObjectId
            };

            await _domainObjectServiceDependencies.RelationService.HydrateDomainObjectReferenceListItemAsync(domainObjectReferenceListItem, relationKey);

            return domainObjectReferenceListItem;
        }

        public async Task<T?> GetFromSystemCodeAsync(string systemCode, CancellationToken cancellationToken = default)
        {
            var domainObject = await WithReadPermissionAsync(() => _repository.GetFromSystemCodeAsync(systemCode, cancellationToken));

            if (domainObject != null)
            {
                await LoadReferencesAsync(domainObject);
            }

            return domainObject;
        }

        public virtual async Task SaveAsync(T domainObject)
        {
            if (domainObject is ISystemObject systemObject && !string.IsNullOrEmpty(systemObject.SystemCode))
            {
                throw new SystemObjectModificationException();
            }

            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.LastModifiedById = _currentUserService.UserId;
            domainObject.LastModifiedByName = _currentUserService.UserName;

            await WithUpdatePermissionAsync(() => _repository.SaveAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(typeof(T), domainObject.Id));
        }

        public async Task SaveIndexAsync(IIndexable domainObject)
        {
            ((DomainObject)domainObject).LastModifiedDateTime = DateTime.Now;
            ((DomainObject)domainObject).LastModifiedById = _currentUserService.UserId;
            ((DomainObject)domainObject).LastModifiedByName = _currentUserService.UserName;

            await WithUpdatePermissionAsync(() => _repository.SaveIndexAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(typeof(T), domainObject.Id));
        }

        public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            return await WithReadPermissionAsync(() => _domainObjectServiceDependencies.RelationService.GetDomainObjectReferencesAsync<T>(domainObjectId));
        }

        #endregion

        #region Private Methods

        protected virtual async Task LoadReferencesAsync(T domainObject)
        {
            var references = await GetDomainObjectReferencesAsync(domainObject.Id);

            foreach (var reference in references)
            {
                var entry = _domainObjectServiceDependencies.RelationResolver.TryGetReferenceRelationEntry(StringKey.From(reference.RelationKey), out var relationEntry);

                relationEntry.DomainProperty?.SetValue(domainObject, reference);
            }

            var referenceLists = await GetDomainObjectReferenceListsAsync(domainObject.Id);

            foreach (var referenceList in referenceLists)
            {
                var entry = _domainObjectServiceDependencies.RelationResolver.TryGetReferenceListRelationEntry(StringKey.From(referenceList.RelationKey), out var relationEntry);

                relationEntry.DomainProperty?.SetValue(domainObject, referenceList);
            }
        }

        private async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId)
        {
            return await WithReadPermissionAsync(() => _domainObjectServiceDependencies.RelationService.GetDomainObjectReferencesAsync<T>(domainObjectId));
        }

        private async Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync(int domainObjectId)
        {
            return await WithReadPermissionAsync(() => _domainObjectServiceDependencies.RelationService.GetDomainObjectReferenceListsAsync<T>(domainObjectId));
        }

        private async Task<bool> IsSystemObjectAsync(int domainObjectId)
        {
            return typeof(T).IsAssignableFrom(typeof(ISystemObject)) && await _repository.IsSystemObjectAsync(domainObjectId);
        }

        private async Task CreateWithoutRightsCheckAsync(T domainObject)
        {
            domainObject.CreationDate = DateTime.Now;
            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.CreatedById = _currentUserService.UserId;
            domainObject.CreatedByName = _currentUserService.UserName;
            domainObject.LastModifiedById = _currentUserService.UserId;
            domainObject.LastModifiedByName = _currentUserService.UserName;

            await _repository.CreateAsync(domainObject);
            await _domainObjectEventBus.PublishAsync(new DomainObjectCreatedEvent(typeof(T), domainObject.Id));
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
            if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _currentUserService.UserId!.Value, permissionType))
                throw new UserPermissionException(typeof(T), permissionType);

            return await action();
        }

        private async Task WithPermissionAsync(UserRolePermissionType permissionType, Func<Task> action)
        {
            if (!await CheckRightAsync(UserRolePermissionKeyHelper.GetPermissionKey<T>(), _currentUserService.UserId!.Value, permissionType))
                throw new UserPermissionException(typeof(T), permissionType);

            await action();
        }

        protected Task WithCreationPermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Create, action);
        protected Task<T> WithCreationPermissionAsync(Func<Task<T>> action) => WithPermissionAsync(UserRolePermissionType.Create, action);
        protected Task<U> WithReadPermissionAsync<U>(Func<Task<U>> action) => WithPermissionAsync(UserRolePermissionType.Read, action);
        protected Task WithUpdatePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Update, action);
        protected Task WithUpdateDomainObjectReferencePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Update, action);
        protected Task WithDeletePermissionAsync(Func<Task> action) => WithPermissionAsync(UserRolePermissionType.Delete, action);

        #endregion
    }
}
