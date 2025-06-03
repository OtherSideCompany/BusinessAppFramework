using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Mail;
using OtherSideCore.Application.Repository;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
   {
      #region Fields

      protected readonly IRepository<T> _repository;
      protected readonly IUserContext _userContext;
      protected readonly IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected readonly IUserDialogService _userDialogService;
      protected readonly IDomainObjectFileService _domainObjectFileService;
      protected readonly IPasswordService _passwordService;
      protected readonly IMailService _mailService;

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
         _repository = repository;
         _userContext = domainObjectServiceDependencies.UserContext;
         _domainObjectServiceFactory = domainObjectServiceDependencies.DomainObjectServiceFactory;
         _userDialogService = domainObjectServiceDependencies.UserDialogService;
         _domainObjectFileService = domainObjectServiceDependencies.DomainObjectFileService;
      }

      #endregion

      #region Public Methods

      public async Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.ExistsAsync(domainObjectId, cancellationToken);
      }

      public async Task<List<T>> GetAllAsync(DomainObject? parent, CancellationToken cancellationToken = default)
      {
         return await _repository.GetAllAsync(parent, cancellationToken);
      }

      public virtual async Task CreateAsync(T domainObject, DomainObject? parent)
      {
         if (domainObject is IIndexable indexableDomainObject && parent != null)
         {
            indexableDomainObject.Index = ((IIndexableRepository)_repository).GetNewIndex(parent);
         }

         if (domainObject is ICommentThreadContainer commentThreadContainer)
         {
            var commentThreadService = _domainObjectServiceFactory.CreateDomainObjectService<CommentThread>();
            var commentThread = await commentThreadService.CreateAsync(domainObject);

            commentThreadContainer.CommentThread = commentThread;
         }

         await _repository.CreateAsync(domainObject, parent, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
         await _domainObjectServiceFactory.DomainObjectEventPublisher.PublishAsync(new DomainObjectCreatedEvent(domainObject));
      }

      public virtual async Task<T> CreateAsync(DomainObject? parent)
      {
         var domainObject = new T();
         await CreateAsync(domainObject, parent);
         return domainObject;
      }

      public virtual async Task<bool> DeleteAsync(T domainObject)
      {
         var deletedDomainObjectId = domainObject.Id;

         int? commentThreadId = null;
         var commentThreadService = (ICommentThreadService)_domainObjectServiceFactory.CreateDomainObjectService<CommentThread>();

         if (domainObject is ICommentThreadContainer commentThreadContainer)
         {            
            commentThreadId = await commentThreadService.GetCommentThreadIdAsync(commentThreadContainer);
         }

         try
         {
            await _domainObjectServiceFactory.DomainObjectEventPublisher.PublishAsync(new DomainObjectDeletingEvent(domainObject));

            await _repository.DeleteAsync(domainObject);

            if (commentThreadId.HasValue)
            {
               await commentThreadService.DeleteCommentThreadAsync(commentThreadId.Value);
            }

            await _domainObjectServiceFactory.DomainObjectEventPublisher.PublishAsync(new DomainObjectDeletedEvent(domainObject, deletedDomainObjectId));

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

      public async Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.GetAsync(domainObjectId, cancellationToken);
      }

      public virtual async Task SaveAsync(T domainObject)
      {
         await _repository.SaveAsync(domainObject, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
         await _domainObjectServiceFactory.DomainObjectEventPublisher.PublishAsync(new DomainObjectSavedEvent(domainObject));
      }

      public async Task SaveIndexAsync(IIndexable domainObject)
      {
         await _repository.SaveIndexAsync(domainObject, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
         await _domainObjectServiceFactory.DomainObjectEventPublisher.PublishAsync(new DomainObjectSavedEvent((DomainObject)domainObject));
      }

      public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.GetDomainObjectReferencesAsync(domainObjectId, cancellationToken);
      }

      public async Task<DomainObjectReference> CreateDomainObjectReferenceAsync(int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken = default)
      {
         return await _repository.CreateDomainObjectReferenceAsync(domainObjectId, domainObjectReferenceId, referenceType, cancellationToken);
      }

      public async Task DeleteDomainObjectReferenceAsync(int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default)
      {
         await _repository.DeleteDomainObjectReferenceAsync(domainObjectId, domainObjectReference, cancellationToken);
      }

      public virtual async Task SetParentAsync(T domainObject, DomainObject parent, CancellationToken cancellationToken = default)
      {
         await _repository.SetParentAsync(domainObject, parent, cancellationToken);
      }

      public virtual async Task<int?> GetParentIdAsync<U>(T domainObject, CancellationToken cancellationToken = default) where U : DomainObject
      {
         return await _repository.GetParentIdAsync<U>(domainObject, cancellationToken);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
