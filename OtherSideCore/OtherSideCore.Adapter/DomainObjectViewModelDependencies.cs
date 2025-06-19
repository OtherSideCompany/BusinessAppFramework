using AutoMapper;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter
{
   public class DomainObjectViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IGlobalDataService GlobalDataService { get; }
      public IMapper Mapper { get; }
      public IDomainObjectViewModelFactory DomainObjectViewModelFactory { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectViewModelDependencies(
         IGlobalDataService globalDataService,
         IMapper mapper,
         IDomainObjectViewModelFactory domainObjectViewModelFactory)
      {
         GlobalDataService = globalDataService;
         Mapper = mapper;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
