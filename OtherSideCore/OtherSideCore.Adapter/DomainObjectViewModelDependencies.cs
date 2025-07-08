using AutoMapper;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.Factories;
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
      public ILocalizationService LocalizationService { get; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; }
      public IDomainObjectQueryServiceFactory DomainObjectQueryServiceFactory { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectViewModelDependencies(
         IGlobalDataService globalDataService,
         IMapper mapper,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         ILocalizationService localizationService,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory)
      {
         GlobalDataService = globalDataService;
         Mapper = mapper;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         LocalizationService = localizationService;
         DomainObjectSearchFactory = domainObjectSearchFactory;
         DomainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
