using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public abstract class DomainObjectInteractionService : IDomainObjectInteractionService
   {
      #region Fields      


      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectInteractionService()
      {
         
      }

      #endregion

      #region Public Methods

      public abstract void RegisterDomainObjectBrowserViewModelType(Enum enumValue, Func<object> factory);
      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel(Enum enumValue);


      // OLD
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel?> DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();


      public abstract IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel<T>() where T : DomainObject, new();

      public abstract IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(Type type);           

      public abstract List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel);

      public virtual async Task<IDomainObjectTreeViewNode> CreateDomainObjectTreeViewNodeAsync(DomainObjectViewModel domainObjectViewModel, IUserDialogService userDialogService, IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         var domainObjectType = domainObjectViewModel.DomainObject.GetType();
         var genericType = typeof(DomainObjectTreeViewNode<>).MakeGenericType(domainObjectType);

         var treeViewNode = (IDomainObjectTreeViewNode)Activator.CreateInstance(genericType, domainObjectViewModel, userDialogService, this, domainObjectServiceFactory);
         await treeViewNode.InitializeAsync();

         return treeViewNode;
      }

      public abstract DomainObjectTreeViewModel CreateTreeViewModel<T>() where T : DomainObject, new();

      public abstract Task DisplayDomainObjectAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectBrowserAsync(Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObject domainObject, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(DomainObjectViewModel domainObjectViewModel, DisplayType displayType);
      public abstract Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType);

      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(DomainObjectViewModel domainObjectViewModel) where T : DomainObject, new();
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(Type domainObjectType, DomainObjectViewModel domainObjectViewModel);
      public abstract Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(int domainObjectId) where T : DomainObject, new();


      #endregion

      #region Private Methods



      #endregion
   }
}
