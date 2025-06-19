using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectEditorViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IDomainObjectInteractionService DomainObjectInteractionService { get; }
      public IUserDialogService UserDialogService { get; }
      public IWindowService WindowService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEditorViewModelDependencies(
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectInteractionService domainObjectInteractionService,
         IUserDialogService userDialogService,
         IWindowService windowService)
      {
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectInteractionService = domainObjectInteractionService;
         UserDialogService = userDialogService;
         WindowService = windowService;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
