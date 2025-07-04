using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
    public class DomainObjectBrowserViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDomainObjectsSearchViewModelFactory DomainObjectsSearchViewModelFactory { get; }
      public IDomainObjectSearchResultViewModelFactory DomainObjectSearchResultViewModelFactory { get; }
      public IDomainObjectInteractionService DomainObjectInteractionService { get; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IWindowService WindowService { get; }
      public IUserDialogService UserDialogService { get; }
      public ILocalizationService LocalizationService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowserViewModelDependencies(
         IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
         IDomainObjectInteractionService domainObjectInteractionFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IWindowService windowService,
         IUserDialogService userDialogService,
         ILocalizationService localizationService)
      {
         DomainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         DomainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
         DomainObjectInteractionService = domainObjectInteractionFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
         WindowService = windowService;
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
