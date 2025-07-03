using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowserViewModelDependencies(
         IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
         IDomainObjectInteractionService domainObjectInteractionFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IWindowService windowService)
      {
         DomainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         DomainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
         DomainObjectInteractionService = domainObjectInteractionFactory;
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
