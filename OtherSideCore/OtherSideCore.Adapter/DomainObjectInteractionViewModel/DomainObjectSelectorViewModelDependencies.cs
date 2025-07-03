using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectSelectorViewModelDependencies : DomainObjectBrowserViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IUserDialogService UserDialogService { get; }
      public WorkspaceFactory WorkspaceFactory { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSelectorViewModelDependencies(
         DomainObjectBrowserViewModelDependencies domainObjectBrowserViewModelDependencies,
         IUserDialogService userDialogService,
         WorkspaceFactory workspaceFactory) :
         base(
            domainObjectBrowserViewModelDependencies.DomainObjectsSearchViewModelFactory,
            domainObjectBrowserViewModelDependencies.DomainObjectSearchResultViewModelFactory,
            domainObjectBrowserViewModelDependencies.DomainObjectInteractionService,
            domainObjectBrowserViewModelDependencies.DomainObjectServiceFactory,
            domainObjectBrowserViewModelDependencies.WindowService)
      {
         UserDialogService = userDialogService;
         WorkspaceFactory = workspaceFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
