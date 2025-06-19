using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectTreeViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDomainObjectInteractionService DomainObjectInteractionService { get; }
      public IDomainObjectViewModelFactory DomainObjectViewModelFactory { get; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; }
      public IUserDialogService UserDialogService { get; }
      public IUserContext UserContext { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeViewModelDependencies(
         IDomainObjectInteractionService domainObjectInteractionService,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IUserDialogService userDialogService,
         IUserContext userContext)
      {
         DomainObjectInteractionService = domainObjectInteractionService;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectSearchFactory = domainObjectSearchFactory;
         UserDialogService = userDialogService;
         UserContext = userContext;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
