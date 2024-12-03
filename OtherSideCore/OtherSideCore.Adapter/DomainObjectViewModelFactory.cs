using AutoMapper;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter
{
   public abstract class DomainObjectViewModelFactory : IDomainObjectViewModelFactory
   {
      #region Fields

      protected IGlobalDataService _globalDataService;
      protected IMapper _mapper;
      protected IUserContext _userContext;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectViewModelFactory(
         IGlobalDataService globalDataService, 
         IMapper mapper, 
         IUserContext userContext)
      {
         _globalDataService = globalDataService;
         _mapper = mapper;
         _userContext = userContext;
      }

      #endregion

      #region Public Methods

      public abstract DomainObjectViewModel CreateViewModel(DomainObject domainObject);

      #endregion

      #region Private Methods



      #endregion
   }
}
