using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Reflection;

namespace OtherSideCore.Adapter.Services
{
   public class DomainObjectInteractionService : IDomainObjectInteractionService
   {
      #region Fields

      private DomainObjectInteractionServiceDependencies _domainObjectInteractionServiceDependencies;

      private StringKeyBasedFactory _domainObjectBrowserViewModelFactory;
      private StringKeyBasedFactory _domainObjectDetailsEditorViewModelFactory;
      private StringKeyBasedFactory _domainObjectEditorViewModelFactory;
      private StringKeyBasedFactory _domainObjectSelectorViewModelFactory;
      private TypeBasedFactory _treeNodeFactory;
      private StringKeyBasedFactory _treeViewModelFactory;
      private StringKeyBasedFactory _multiSelectListViewModelFactory;

      #endregion

      #region Properties

      public DomainObjectInteractionMappingRegistry DefaultDomainObjectInteractionMappingRegistry { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectInteractionService(DomainObjectInteractionServiceDependencies domainObjectInteractionServiceDependencies)
      {
         _domainObjectInteractionServiceDependencies = domainObjectInteractionServiceDependencies;

         _domainObjectBrowserViewModelFactory = new StringKeyBasedFactory();
         _domainObjectDetailsEditorViewModelFactory = new StringKeyBasedFactory();
         _domainObjectEditorViewModelFactory = new StringKeyBasedFactory();
         _domainObjectSelectorViewModelFactory = new StringKeyBasedFactory();
         _treeNodeFactory = new TypeBasedFactory();
         _treeViewModelFactory = new StringKeyBasedFactory();
         _multiSelectListViewModelFactory = new StringKeyBasedFactory();

         DefaultDomainObjectInteractionMappingRegistry = new DomainObjectInteractionMappingRegistry();
      }

      #endregion

      #region Public Methods

      public void RegisterDomainObjectBrowserViewModel(StringKey key, Func<IDomainObjectBrowserViewModel> factory)
      {
         _domainObjectBrowserViewModelFactory.Register(key, factory);
      }

      public IDomainObjectBrowserViewModel CreateDomainObjectBrowserViewModel(StringKey key)
      {
         return (IDomainObjectBrowserViewModel)_domainObjectBrowserViewModelFactory.Create(key);
      }

      public void RegisterDomainObjectEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory)
      {
         _domainObjectEditorViewModelFactory.Register(key, args =>
         {
            var search = (DomainObjectViewModel)args[0]!;
            return factory(search);
         });
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetBySearchResultType(domainObjectSearchResult.GetType(), out var mapping))
         {
            throw new InvalidOperationException($"No editor mapping registered for {domainObjectSearchResult.GetType().Name}");
         }

         return await CreateDomainObjectEditorViewModelAsync(mapping.EditorKey, mapping.DomainObjectType, domainObjectSearchResult.DomainObjectId);
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetByTreeNodeViewModelType(domainObjectTreeNodeViewModel.GetType(), out var mapping))
         {
            throw new InvalidOperationException($"No editor mapping registered for {domainObjectTreeNodeViewModel.GetType().Name}");
         }

         return await CreateDomainObjectEditorViewModelAsync(mapping.EditorKey, domainObjectTreeNodeViewModel.DomainObjectViewModel);
      }

      public async Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectEditor = (IDomainObjectEditorViewModel)_domainObjectEditorViewModelFactory.Create(key, domainObjectViewModel);

         await domainObjectEditor.InitializeAsync();

         return domainObjectEditor;
      }

      public async Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(StringKey key, DomainObject domainObject)
      {
         var viewModel = _domainObjectInteractionServiceDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
         return await CreateDomainObjectEditorViewModelAsync(key, viewModel);
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId)
      {
         var method = GetType().GetMethod(nameof(CreateDomainObjectEditorViewModelAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                               .MakeGenericMethod(domainObjectType);

         var task = (Task)method.Invoke(this, new object[] { key, domainObjectId })!;
         await task.ConfigureAwait(false);

         var resultProperty = task.GetType().GetProperty("Result")!;
         return (IDomainObjectEditorViewModel)resultProperty.GetValue(task)!;
      }

      public void RegisterDomainObjectDetailsEditorViewModel(StringKey key, Func<DomainObjectViewModel, IDomainObjectEditorViewModel> factory)
      {
         _domainObjectDetailsEditorViewModelFactory.Register(key, args =>
         {
            var search = (DomainObjectViewModel)args[0]!;
            return factory(search);
         });
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(DomainObjectSearchResult domainObjectSearchResult)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetBySearchResultType(domainObjectSearchResult.GetType(), out var mapping))
         {
            throw new InvalidOperationException($"No editor mapping registered for {domainObjectSearchResult.GetType().Name}");
         }

         if (mapping.DetailsEditorKey == null)
         {
            _domainObjectInteractionServiceDependencies.UserDialogService.Error($"Aucun éditeur détaillé par défaut défini pour l'entité sélectionnée");
            return null;
         }
         else
         {
            return await CreateDomainObjectDetailsEditorViewModelAsync(mapping.DetailsEditorKey, mapping.DomainObjectType, domainObjectSearchResult.DomainObjectId);
         }         
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(IDomainObjectTreeNodeViewModel domainObjectTreeNodeViewModel)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetByTreeNodeViewModelType(domainObjectTreeNodeViewModel.GetType(), out var mapping))
         {
            throw new InvalidOperationException($"No editor mapping registered for {domainObjectTreeNodeViewModel.GetType().Name}");
         }

         if (mapping.DetailsEditorKey == null)
         {
            _domainObjectInteractionServiceDependencies.UserDialogService.Error($"Aucun éditeur détaillé par défaut défini pour l'entité sélectionnée");
            return null;
         }
         else
         {
            return await CreateDomainObjectDetailsEditorViewModelAsync(mapping.DetailsEditorKey, domainObjectTreeNodeViewModel.DomainObjectViewModel);
         }
      }

      public async Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectEditorViewModel = (IDomainObjectEditorViewModel)_domainObjectDetailsEditorViewModelFactory.Create(key, domainObjectViewModel);

         await domainObjectEditorViewModel.InitializeAsync();
         await domainObjectEditorViewModel.LoadNestedStructuresAsync();

         return domainObjectEditorViewModel;
      }

      public async Task<IDomainObjectEditorViewModel> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, DomainObject domainObject)
      {
         var viewModel = _domainObjectInteractionServiceDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
         return await CreateDomainObjectDetailsEditorViewModelAsync(key, viewModel);
      }

      public async Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync(StringKey key, Type domainObjectType, int domainObjectId)
      {
         var method = GetType().GetMethod(nameof(CreateDomainObjectDetailsEditorViewModelAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                               .MakeGenericMethod(domainObjectType);

         var task = (Task)method.Invoke(this, new object[] { key, domainObjectId })!;
         await task.ConfigureAwait(false);

         var resultProperty = task.GetType().GetProperty("Result")!;
         return (IDomainObjectEditorViewModel)resultProperty.GetValue(task)!;
      }

      public void RegisterDomainObjectSelectorViewModel(StringKey key, Func<IDomainObjectSelectorViewModel> factory)
      {
         _domainObjectSelectorViewModelFactory.Register(key, factory);
      }

      public IDomainObjectSelectorViewModel CreateDomainObjectSelectorViewModel(StringKey key)
      {
         return (IDomainObjectSelectorViewModel)_domainObjectSelectorViewModelFactory.Create(key);
      }

      public void RegisterTreeViewModel(StringKey key, Func<DomainObjectTreeViewModel> factory)
      {
         _treeViewModelFactory.Register(key, factory);
      }

      public DomainObjectTreeViewModel CreateTreeViewModel(StringKey key)
      {
         return (DomainObjectTreeViewModel)_treeViewModelFactory.Create(key);
      }

      public void RegisterMultiSelectListViewModel(StringKey key, Func<IMultiSelectListViewModel> factory)
      {
         _multiSelectListViewModelFactory.Register(key, factory);
      }

      public IMultiSelectListViewModel CreateMultiSelectListViewModel(StringKey key)
      {
         return (IMultiSelectListViewModel)_multiSelectListViewModelFactory.Create(key);
      }

      public void RegisterTreeNodeViewModel(Type type, Func<DomainObjectViewModel, IDomainObjectTreeNodeViewModel> factory)
      {
         _treeNodeFactory.Register(type, args =>
         {
            var viewModel = (DomainObjectViewModel)args[0]!;
            return factory(viewModel);
         });
      }

      public async Task<IDomainObjectTreeNodeViewModel> CreateDomainObjectTreeNodeViewModelAsync(DomainObjectViewModel domainObjectViewModel)
      {
         var viewModel = (IDomainObjectTreeNodeViewModel)_treeNodeFactory.CreateFromType(domainObjectViewModel.GetType(), domainObjectViewModel);
         await viewModel.InitializeAsync();
         return viewModel;
      }

      public async Task DisplayDomainObjectWorkspaceAsync(int? domainObjectId, Type domainObjectType)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetByDomainObjectType(domainObjectType, out var mapping))
         {
            throw new InvalidOperationException($"No default mapping registered for {domainObjectType.Name}");            
         }

         if (mapping.WorkspaceKey == null)
         {
            _domainObjectInteractionServiceDependencies.UserDialogService.Error($"Aucun espace de travail par défaut défini pour l'entité sélectionnée");
            return;
         }

         var workspace = _domainObjectInteractionServiceDependencies.WorkspaceFactory.CreateWorkspace(mapping.WorkspaceKey);

         if (domainObjectId != null && workspace is SingleDomainObjectBrowserWorkspace)
         {
            await workspace.InitializeAsync(domainObjectId);
         }
         else
         {
            await workspace.InitializeAsync();
         }

         var session = _domainObjectInteractionServiceDependencies.WindowService.DisplayView(mapping.WorkspaceKey, "", workspace, DisplayType.SubWindow);
         await session.WhenClosed;
         workspace.Dispose();
      }

      public async Task DisplayDomainObjectDetailsEditorViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType)
      {
         if (!DefaultDomainObjectInteractionMappingRegistry.TryGetByDomainObjectType(domainObjectType, out var mapping))
         {
            throw new InvalidOperationException($"No default mapping registered for {domainObjectType.Name}");
         }

         if (mapping.DetailsEditorKey == null)
         {
            _domainObjectInteractionServiceDependencies.UserDialogService.Error($"Aucun éditeur détaillé par défaut défini pour l'entité sélectionnée");
            return;
         }

         var domainObjectDetailsEditorViewModel = CreateDomainObjectDetailsEditorViewModelAsync(mapping.DetailsEditorKey, domainObjectType, domainObjectId);

         var session = _domainObjectInteractionServiceDependencies.WindowService.DisplayView(mapping.DetailsEditorKey, "", domainObjectDetailsEditorViewModel, DisplayType.Modal);
         await session.WhenClosed;
         domainObjectDetailsEditorViewModel.Dispose();
      }

      public Task DisplayDomainObjectTreeViewAsync(int domainObjectId, Type domainObjectType, DisplayType displayType)
      {
         _domainObjectInteractionServiceDependencies.UserDialogService.Error($"Aucune vue en arborescence par défaut définie pour l'entité sélectionnée");
         return Task.CompletedTask;
      }

      public List<DomainObjectReferenceSelectorViewModel> GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel domainObjectViewModel)
      {
         List<DomainObjectReferenceSelectorViewModel> domainObjectSelectorViewModels = new List<DomainObjectReferenceSelectorViewModel>();

         /*foreach (var type in _referenceMapFactory.GetDomainObjectReferenceMap().GetReferenceTypes(domainObjectViewModel.DomainObject.GetType()))
         {
            DomainObjectReferenceSelectorViewModel domainObjectReferenceSelectorViewModel = null;

            if (type == typeof(ClientOrder))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Commande client", CreateDomainObjectSelectorViewModel(type));
            }
            else if (type == typeof(ClientOrderDispatch))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Dispatch", CreateDomainObjectSelectorViewModel(type));
            }
            else if (type == typeof(Opportunity))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Opportunité", CreateDomainObjectSelectorViewModel(type));
            }
            else if (type == typeof(Project))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Projet", CreateDomainObjectSelectorViewModel(type));
            }
            else if (type == typeof(ProviderOrder))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Commande fournisseur", CreateDomainObjectSelectorViewModel(type));
            }
            else if (type == typeof(ProviderQuote))
            {
               domainObjectReferenceSelectorViewModel = new DomainObjectReferenceSelectorViewModel(type, "Demande de prix fournisseur", CreateDomainObjectSelectorViewModel(type));
            }
            else
            {
               throw new ArgumentException($"Cannot handle Domain Object Type {domainObjectViewModel.DomainObject.GetType()} in GetDomainObjectReferenceSelectorViewModels");
            }

            domainObjectSelectorViewModels.Add(domainObjectReferenceSelectorViewModel);
         }*/

         return domainObjectSelectorViewModels;
      }      

      #endregion

      #region Private Methods

      private async Task<IDomainObjectEditorViewModel?> CreateDomainObjectEditorViewModelAsync<T>(StringKey key, int domainObjectId) where T : DomainObject, new()
      {
         var (success, domainObject) = await DomainObjectServiceHelper.TryGetAsync(
               domainObjectId,
               _domainObjectInteractionServiceDependencies.DomainObjectServiceFactory.CreateDomainObjectService<T>(),
               _domainObjectInteractionServiceDependencies.UserDialogService,
               _domainObjectInteractionServiceDependencies.LocalizationService);

         if (!success)
         {
            return null;
         }

         var viewModel = _domainObjectInteractionServiceDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
         return await CreateDomainObjectEditorViewModelAsync(key, viewModel);
      }

      private async Task<IDomainObjectEditorViewModel?> CreateDomainObjectDetailsEditorViewModelAsync<T>(StringKey key, int domainObjectId) where T : DomainObject, new()
      {
         var (success, domainObject) = await DomainObjectServiceHelper.TryGetAsync(
               domainObjectId,
               _domainObjectInteractionServiceDependencies.DomainObjectServiceFactory.CreateDomainObjectService<T>(),
               _domainObjectInteractionServiceDependencies.UserDialogService,
               _domainObjectInteractionServiceDependencies.LocalizationService);

         if (!success)
         {
            return null;
         }

         var viewModel = _domainObjectInteractionServiceDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
         return await CreateDomainObjectDetailsEditorViewModelAsync(key, viewModel);
      }

      #endregion
   }
}
