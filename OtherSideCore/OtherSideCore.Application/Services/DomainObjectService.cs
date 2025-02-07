using OtherSideCore.Application.Repository;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
   {
      #region Fields

      protected readonly IRepository<T> _repository;
      protected readonly IUserContext _userContext;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectService(IRepository<T> repository, IUserContext userContext)
      {
         _repository = repository;
         _userContext = userContext;
      }

      #endregion

      #region Public Methods

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

         await _repository.CreateAsync(domainObject, parent, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
      }

      public virtual async Task<T> CreateAsync(DomainObject? parent)
      {
         var domainObject = new T();
         await CreateAsync(domainObject, parent);
         return domainObject;
      }

      public virtual async Task DeleteAsync(T domainObject)
      {
         await _repository.DeleteAsync(domainObject);
      }

      public async Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.GetAsync(domainObjectId, cancellationToken);
      }

      public virtual async Task SaveAsync(T domainObject)
      {
         await _repository.SaveAsync(domainObject, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
      }

      public async Task SaveIndexAsync(IIndexable domainObject)
      {
         await _repository.SaveIndexAsync(domainObject, _userContext.Id, _userContext.FirstName + " " + _userContext.LastName);
      }

      public async Task<List<DomainObjectReference>> GetDomainObjectReferencesAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.GetDomainObjectReferences(domainObjectId, cancellationToken);
      }

      public async Task<DomainObjectReference> CreateDomainObjectReferenceAsync(int domainObjectId, int domainObjectReferenceId, Type referenceType, CancellationToken cancellationToken = default)
      {
         return await _repository.CreateDomainObjectReferenceAsync(domainObjectId, domainObjectReferenceId, referenceType, cancellationToken);
      }

      public async Task DeleteDomainObjectReferenceAsync(int domainObjectId, DomainObjectReference domainObjectReference, CancellationToken cancellationToken = default)
      {
         await _repository.DeleteDomainObjectReferenceAsync(domainObjectId, domainObjectReference, cancellationToken);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
