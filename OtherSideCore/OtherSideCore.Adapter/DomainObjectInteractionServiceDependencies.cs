using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter
{
   public class DomainObjectInteractionServiceDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IDomainObjectViewModelFactory DomainObjectViewModelFactory { get; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; }
      public IServiceProvider ServiceProvider { get; }
      public IUserDialogService UserDialogService { get; }
      public ILocalizationService LocalizationService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectInteractionServiceDependencies(
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IServiceProvider serviceProvider,
         IUserDialogService userDialogService,
         ILocalizationService localizationService)
      {
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         DomainObjectSearchFactory = domainObjectSearchFactory;
         ServiceProvider = serviceProvider;
         UserDialogService = userDialogService;
         LocalizationService = localizationService;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
