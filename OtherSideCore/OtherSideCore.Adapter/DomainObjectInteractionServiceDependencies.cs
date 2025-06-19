using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;

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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectInteractionServiceDependencies(
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IServiceProvider serviceProvider)
      {
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         DomainObjectSearchFactory = domainObjectSearchFactory;
         ServiceProvider = serviceProvider;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
