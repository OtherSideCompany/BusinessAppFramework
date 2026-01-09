using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Exceptions;
using OtherSideCore.Application.Mail;
using OtherSideCore.Application.Repository;
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
            _domainObjectFileService = domainObjectServiceDependencies.DomainObjectFileService;
            _domainObjectEventBus = domainObjectServiceDependencies.DomainObjectEventBus;
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
            var domainObjects = await WithReadPermissionAsync(() => _repository.GetAllAsync(parent, cancellationToken));

            foreach (var domainObject in domainObjects)
            {
                await LoadReferencesAsync(domainObject);
            }

            return domainObjects;
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

                _domainObjectFileService.TryDeleteAssociatedFolder(domainObjectId);

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

        public async Task<List<object>> GetChildrenAsync(int parentId, string relationKey, CancellationToken cancellationToken = default)
        {
            return await _repository.GetChildrenAsync(parentId, relationKey, cancellationToken);
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

            await _repository.HydrateDomainObjectReferenceAsync(domainObjectReference);

            return domainObjectReference;
        }

        public async Task<DomainObjectReferenceListItem> GetHydratedDomainObjectReferenceListItem(int domainObjectId, string relationKey)
        {
            var domainObjectReferenceListItem = new DomainObjectReferenceListItem
            {
                DomainObjectId = domainObjectId
            };

            await _repository.HydrateDomainObjectReferenceListItemAsync(domainObjectReferenceListItem, relationKey);

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
            if (domainObject is ISystemObject systemObject && !String.IsNullOrEmpty(systemObject.SystemCode))
            {
                throw new SystemObjectModificationException();
            }

            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.LastModifiedById = _userContext.Id;
            domainObject.LastModifiedByName = _userContext.GetName();

            await WithUpdatePermissionAsync(() => _repository.SaveAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(typeof(T), domainObject.Id));
        }

        public async Task SaveIndexAsync(IIndexable domainObject)
        {
            ((DomainObject)domainObject).LastModifiedDateTime = DateTime.Now;
            ((DomainObject)domainObject).LastModifiedById = _userContext.Id;
            ((DomainObject)domainObject).LastModifiedByName = _userContext.GetName();

            await WithUpdatePermissionAsync(() => _repository.SaveIndexAsync(domainObject));
            await _domainObjectEventBus.PublishAsync(new DomainObjectSavedEvent(typeof(T), domainObject.Id));
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

        public virtual async Task<int?> GetParentIdAsync<U>(int childDomainObjectId, CancellationToken cancellationToken = default) where U : DomainObject
        {
            return await WithReadPermissionAsync(() => _repository.GetParentIdAsync<U>(childDomainObjectId, cancellationToken));
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
            return await WithReadPermissionAsync(() => _repository.GetDomainObjectReferencesAsync(domainObjectId));
        }

        private async Task<List<DomainObjectReferenceList>> GetDomainObjectReferenceListsAsync(int domainObjectId)
        {
            return await WithReadPermissionAsync(() => _repository.GetDomainObjectReferenceListsAsync(domainObjectId));
        }

        private async Task<bool> IsSystemObjectAsync(int domainObjectId)
        {
            return typeof(T).IsAssignableFrom(typeof(ISystemObject)) && await _repository.IsSystemObjectAsync(domainObjectId);
        }

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
