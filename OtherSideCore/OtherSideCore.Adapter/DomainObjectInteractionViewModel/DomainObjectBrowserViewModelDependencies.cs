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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectBrowserViewModelDependencies(
         IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
         IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
         IDomainObjectInteractionService domainObjectInteractionFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         DomainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         DomainObjectSearchResultViewModelFactory = domainObjectSearchResultViewModelFactory;
         DomainObjectInteractionService = domainObjectInteractionFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
