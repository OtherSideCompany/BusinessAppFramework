using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Services;
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
   public class MultiSelectListViewModelDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IDomainObjectViewModelFactory DomainObjectViewModelFactory { get; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; }
      public IUserDialogService UserDialogService { get; }
      public ILocalizationService LocalizationService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MultiSelectListViewModelDependencies(
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IUserDialogService userDialogService,
         ILocalizationService localizationService)
      {
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         DomainObjectServiceFactory = domainObjectServiceFactory;
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
