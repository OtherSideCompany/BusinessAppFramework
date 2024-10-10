using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Services
{
   public class DomainObjectService<T> : IDomainObjectService<T> where T : DomainObject, new()
   {
      #region Fields

      protected readonly IRepository<T> _repository;
      protected readonly IUserContext _userContext;
      protected readonly IGlobalDataService _globalDataService;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectService(IRepository<T> repository, IUserContext userContext, IGlobalDataService globalDataService)
      {
         _repository = repository;
         _userContext = userContext;
         _globalDataService = globalDataService;
      }

      #endregion

      #region Public Methods

      public virtual async Task CreateAsync(T domainObject)
      {
         await _repository.CreateAsync(domainObject, _userContext.Id);
      }

      public async Task DeleteAsync(T domainObject)
      {
         await _repository.DeleteAsync(domainObject);
      }

      public async Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return await _repository.GetAsync(domainObjectId, cancellationToken);
      }

      public async Task LoadAsync(T domainObject)
      {
         await _repository.LoadAsync(domainObject);
      }

      public async Task SaveAsync(T domainObject)
      {
         await _repository.SaveAsync(domainObject, _userContext.Id);
      }

      public async Task LoadTrackingInfosAsync(T domainObject)
      {
         await _repository.LoadTrackingInfos(domainObject);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
