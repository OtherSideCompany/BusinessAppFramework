using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Relations;
using OtherSideCore.Adapter.Services;
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
      public IDomainObjectViewModelFactory DomainObjectViewModelFactory { get; }
      public IPropertyEditorFactory PropertyEditorFactory { get; }
      public ILocalizationService LocalizationService { get; }
      public IDomainObjectEntityTypeMap DomainObjectEntityTypeMap { get; }
      public IRelationResolver RelationResolver { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEditorViewModelDependencies(
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectInteractionService domainObjectInteractionService,
         IUserDialogService userDialogService,
         IWindowService windowService,
         IDomainObjectViewModelFactory domainObjectViewModelFactory,
         IPropertyEditorFactory propertyEditorFactory,
         ILocalizationService localizationService,
         IDomainObjectEntityTypeMap domainObjectEntityTypeMap,
         IRelationResolver relationResolver)
      {
         DomainObjectServiceFactory = domainObjectServiceFactory;
         DomainObjectInteractionService = domainObjectInteractionService;
         UserDialogService = userDialogService;
         WindowService = windowService;
         DomainObjectViewModelFactory = domainObjectViewModelFactory;
         PropertyEditorFactory = propertyEditorFactory;
         LocalizationService = localizationService;
         DomainObjectEntityTypeMap = domainObjectEntityTypeMap;
         RelationResolver = relationResolver;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
