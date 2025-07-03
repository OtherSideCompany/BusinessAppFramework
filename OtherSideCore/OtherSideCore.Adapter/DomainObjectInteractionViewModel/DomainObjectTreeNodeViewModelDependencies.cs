using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectTreeNodeViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IUserDialogService UserDialogService { get; set; }
      public IDomainObjectInteractionService DomainObjectInteractionService { get; set; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; set; }
      public IWindowService WindowService { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeNodeViewModelDependencies(
         IUserDialogService userDialogService,
         IDomainObjectInteractionService domainObjectInteractionService,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IWindowService windowService)
      {
         UserDialogService = userDialogService;
         DomainObjectInteractionService = domainObjectInteractionService;
         DomainObjectServiceFactory = domainObjectServiceFactory;
         WindowService = windowService;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
