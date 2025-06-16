using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public abstract class DomainObjectTreeViewModel : ObservableObject, IDisposable, ISavable
   {
      #region Fields

      private readonly SemaphoreSlim _domainObjectEditorViewModelsSemaphore = new SemaphoreSlim(1, 1);

      protected IDomainObjectInteractionService _domainObjectInteractionService;
      private IDomainObjectTreeSearchViewModel _domainObjectTreeSearchViewModel;
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected IDomainObjectSearchFactory _domainObjectSearchFactory;
      protected IUserDialogService _userDialogService;

      private ObservableCollection<IDomainObjectTreeViewNode> _roots;
      private bool _isSelectionLocked;
      private IDomainObjectTreeViewNode _selectedDomainObjectTreeViewNode;
      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;
      private DomainObjectViewModel _ContextViewModel;
      protected bool _isInitializingTree;

      private bool _isLoadingNestedStructures;

      protected IEnumerable<IDomainObjectTreeViewNode> _inlineNodes
      {
         get
         {
            foreach (var node in _roots)
            {
               yield return node;

               foreach (var childNode in node.InlineNodes)
               {
                  yield return childNode;
               }
            }
         }
      }

      #endregion

      #region Properties

      public IDomainObjectViewModelFactory DomainObjectViewModelFactory => _domainObjectViewModelFactory;
      public IDomainObjectServiceFactory DomainObjectServiceFactory => _domainObjectServiceFactory;
      public IUserDialogService UserDialogService => _userDialogService;

      public ObservableCollection<IDomainObjectTreeViewNode> Roots
      {
         get => _roots;
         private set => SetProperty(ref _roots, value);
      }

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      public IDomainObjectTreeViewNode? SelectedDomainObjectTreeViewNode
      {
         get => _selectedDomainObjectTreeViewNode;
         protected set => SetProperty(ref _selectedDomainObjectTreeViewNode, value);
      }

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel => SelectedDomainObjectTreeViewNode?.DomainObjectEditorViewModel;

      public bool IsLoadingNestedStructures
      {
         get => _isLoadingNestedStructures;
         private set => SetProperty(ref _isLoadingNestedStructures, value);
      }

      public DomainObjectViewModel ContextViewModel
      {
         get => _ContextViewModel;
         set => SetProperty(ref _ContextViewModel, value);
      }

      public bool HasUnsavedChanges => _inlineNodes.Select(n => n.DomainObjectEditorViewModel).Any(vm => vm.HasUnsavedChanges);

      #endregion

      #region Events

      public EventHandler PreviewTreeModified;
      public EventHandler TreeModified;

      #endregion

      #region Commands

      public AsyncRelayCommand CreateRootNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand<IDomainObjectTreeViewNode> DupplicateRootNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectTreeViewModel(IUserDialogService userDialogService,
                                       IDomainObjectInteractionService domainObjectInteractionService,
                                       IDomainObjectTreeSearchViewModel domainObjectTreeSearchViewModel,
                                       IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                       IDomainObjectServiceFactory domainObjectServiceFactory,
                                       IDomainObjectSearchFactory domainObjectSearchFactory)
      {
         _domainObjectInteractionService = domainObjectInteractionService;
         _domainObjectTreeSearchViewModel = domainObjectTreeSearchViewModel;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _domainObjectSearchFactory = domainObjectSearchFactory;

         Roots = new ObservableCollection<IDomainObjectTreeViewNode>();

         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         CreateRootNodeAsyncCommand = new AsyncRelayCommand(CreateRootNodeAsync, CanCreateRootNode);
         DupplicateRootNodeAsyncCommand = new AsyncRelayCommand<IDomainObjectTreeViewNode>(DupplicateRootNodeAsync, CanDupplicateRootNodeAsync);
      }


      #endregion

      #region Public Methods

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         foreach (var viewModel in _inlineNodes.Select(n => n.DomainObjectEditorViewModel))
         {
            await viewModel.SaveChangesAsync();
         }
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         foreach (var viewModel in _inlineNodes.Select(n => n.DomainObjectEditorViewModel))
         {
            await viewModel.CancelChangesAsync();
         }
      }

      public async Task InitializeAsync(DomainObjectViewModel domainObjectViewModel)
      {
         _isInitializingTree = true;

         ContextViewModel = domainObjectViewModel;
         await _domainObjectTreeSearchViewModel.SearchAsync(domainObjectViewModel);

         await _domainObjectEditorViewModelsSemaphore.WaitAsync();

         await ConstructTreeAsync(domainObjectViewModel);

         _domainObjectEditorViewModelsSemaphore.Release();

         _isInitializingTree = false;

         NotifyCommandsCanExecuteChanged();
      }

      public async Task SelectNodeAsync(IDomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         if (!IsSelectionLocked)
         {
            if (ProceedSelection(domainObjectTreeViewNode.DomainObjectViewModel))
            {
               SelectedDomainObjectTreeViewNode?.Unselect();

               domainObjectTreeViewNode.Select();
               SelectedDomainObjectTreeViewNode = domainObjectTreeViewNode;

               OnPropertyChanged(nameof(SelectedDomainObjectEditorViewModel));

               IsLoadingNestedStructures = true;

               await SelectedDomainObjectEditorViewModel.LoadNestedStructuresAsync();

               IsLoadingNestedStructures = false;
            }
         }
      }

      public void ExpandAll()
      {
         _inlineNodes.ToList().ForEach(n => n.Expand());
      }

      public void CollapseAll()
      {
         _inlineNodes.ToList().ForEach(n => n.Expand());
      }

      public virtual async Task<IDomainObjectTreeViewNode> AddRootNodeAsync(DomainObjectViewModel domainObjectViewModel)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         var domainObjectRootTreeViewNode = await _domainObjectInteractionService.CreateDomainObjectTreeViewNodeAsync(domainObjectViewModel, _userDialogService, _domainObjectServiceFactory);

         DomainObjectTreeViewModelExtension.InsertNodeInList(domainObjectRootTreeViewNode, Roots);

         RegisterNode(domainObjectRootTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);

         domainObjectRootTreeViewNode.Expand();

         return domainObjectRootTreeViewNode;
      }

      public async Task<IDomainObjectTreeViewNode> AddChildNodeAsync(DomainObjectViewModel domainObjectViewModel, DomainObjectViewModel parentViewModel)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         var domainObjectTreeViewNode = await _domainObjectInteractionService.CreateDomainObjectTreeViewNodeAsync(domainObjectViewModel, _userDialogService, _domainObjectServiceFactory);
         var parentNode = GetNode(parentViewModel);
         parentNode.AddChild(domainObjectTreeViewNode, _isInitializingTree);
         RegisterNode(domainObjectTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);

         return domainObjectTreeViewNode;
      }

      public virtual void RemoveRootNode(IDomainObjectTreeViewNode rootNodeToRemove)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         UnregisterNode(rootNodeToRemove);
         Roots.Remove(rootNodeToRemove);

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      public async Task RemoveChildNodeAsync(IDomainObjectTreeViewNode nodeToRemove, IDomainObjectTreeViewNode parent)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         UnregisterNode(nodeToRemove);
         parent.RemoveChild(nodeToRemove);

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      public void Clear()
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         _inlineNodes.ToList().ForEach(n => UnregisterNode(n));
         _inlineNodes.ToList().ForEach(n => n.Dispose());
         Roots.Clear();

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      public virtual async Task<DomainObject> CreateRootNodeDomainObjectCopyAsync(IDomainObjectTreeViewNode node)
      {
         var dupplicatedDomainObject = await node.DomainObjectEditorViewModel.DupplicateAsync(ContextViewModel?.DomainObject);

         var dupplicatedDomainObjectViewModel = _domainObjectViewModelFactory.CreateViewModel(dupplicatedDomainObject);
         AddRootNodeAsync(dupplicatedDomainObjectViewModel);

         var dupplicatedDomainObjectNode = GetNode(dupplicatedDomainObjectViewModel);

         foreach (var childNode in node.Children)
         {
            await dupplicatedDomainObjectNode.CreateChildNodeDomainObjectCopyAsync(childNode);
         }

         return dupplicatedDomainObject;
      }

      public void InitializeTreeDomainObjectViewModelsProperties()
      {
         _inlineNodes.Select(r => r.DomainObjectViewModel).ToList().ForEach(vm => vm.InitializeProperties());
      }

      public IDomainObjectTreeViewNode GetNode(DomainObjectViewModel domainObjectViewModel)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.Equals(domainObjectViewModel));
      }

      public IDomainObjectTreeViewNode GetNode(DomainObject domainObject)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.DomainObject.Equals(domainObject));
      }

      public virtual void Dispose()
      {
         Clear();
      }

      #endregion

      #region Private Methods  

      protected virtual void RegisterNode(IDomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested += DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted += DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated += DomainObjectTreeViewNode_ChildCreated;
      }

      protected virtual void UnregisterNode(IDomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested -= DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted -= DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated -= DomainObjectTreeViewNode_ChildCreated;
      }

      protected virtual async void DomainObjectTreeViewNode_ChildCreated(object? sender, Domain.DomainObjects.DomainObject e)
      {
         var viewModel = _domainObjectViewModelFactory.CreateViewModel(e);
         await AddChildNodeAsync(viewModel, ((IDomainObjectTreeViewNode)sender).DomainObjectViewModel);
      }

      protected virtual async void DomainObjectTreeViewNode_NodeDeleted(object? sender, IDomainObjectTreeViewNode e)
      {
         await RemoveNodeAsync(e);
      }

      private async Task RemoveNodeAsync(IDomainObjectTreeViewNode nodeToRemove)
      {
         foreach (var rootNode in Roots.ToList())
         {
            if (rootNode.Equals(nodeToRemove))
            {
               RemoveRootNode(nodeToRemove);

               if (SelectedDomainObjectTreeViewNode != null && SelectedDomainObjectTreeViewNode.Equals(nodeToRemove))
               {
                  SelectedDomainObjectTreeViewNode.Unselect();
                  SelectedDomainObjectTreeViewNode = null;
               }

               return;
            }

            await RemoveNodeFromChildrenAsync(nodeToRemove, rootNode);
         }
      }

      private async Task RemoveNodeFromChildrenAsync(IDomainObjectTreeViewNode nodeToRemove, IDomainObjectTreeViewNode parent)
      {
         foreach (var childNode in parent.Children.ToList())
         {
            if (childNode.Equals(nodeToRemove))
            {
               await RemoveChildNodeAsync(nodeToRemove, parent);
               return;
            }

            await RemoveNodeFromChildrenAsync(nodeToRemove, childNode);
         }
      }

      private async void DomainObjectRootTreeViewNode_NodeSelected(object? sender, IDomainObjectTreeViewNode e)
      {
         await SelectNodeAsync(e);
      }

      protected abstract Task ConstructTreeAsync(DomainObjectViewModel domainObjectViewModel);

      private void DomainObjectEditorViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(IDomainObjectEditorViewModel.HasUnsavedChanges)))
         {
            UpdateUnsavedChanges();
         }
      }

      private bool ProceedSelection(DomainObjectViewModel domainObjectViewModel)
      {
         return !domainObjectViewModel.Equals(SelectedDomainObjectTreeViewNode?.DomainObjectViewModel) && !IsSelectionLocked;
      }

      private void UpdateUnsavedChanges()
      {
         OnPropertyChanged(nameof(HasUnsavedChanges));
         NotifyCommandsCanExecuteChanged();

         if (HasUnsavedChanges)
         {
            LockSelection();
         }
         else
         {
            UnlockSelection();
         }
      }

      private void LockSelection()
      {
         IsSelectionLocked = true;
      }

      private void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         CreateRootNodeAsyncCommand.NotifyCanExecuteChanged();
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }

      private bool CanCreateRootNode()
      {
         return ContextViewModel != null;
      }

      protected virtual async Task<IDomainObjectTreeViewNode> CreateRootNodeAsync()
      {
         var domainObject = await CreateRootDomainObjectAsync();

         var viewModel = _domainObjectViewModelFactory.CreateViewModel(domainObject);
         return await AddRootNodeAsync(viewModel);
      }

      private bool CanDupplicateRootNodeAsync(IDomainObjectTreeViewNode? node)
      {
         return node != null && Roots.Contains(node);
      }

      protected virtual async Task<DomainObject> DupplicateRootNodeAsync(IDomainObjectTreeViewNode? node)
      {
         var domainObject = await CreateRootNodeDomainObjectCopyAsync(node);

         await IndexRoots(node.DomainObjectViewModel.DomainObject.GetType());

         return domainObject;
      }

      protected virtual async Task<DomainObject> CreateRootDomainObjectAsync()
      {
         throw new NotImplementedException();
      }

      protected async Task<DomainObject> CreateRootDomainObjectAsync(Type rootType)
      {
         DomainObject domainObject;

         var domainObjectService = (dynamic)_domainObjectServiceFactory.CreateDomainObjectService(rootType);
         domainObject = await domainObjectService.CreateAsync(ContextViewModel.DomainObject);

         await IndexRoots(rootType);

         return domainObject;
      }

      protected async Task IndexRoots(Type rootType)
      {
         var indexableCollection = Roots.Select(c => c.DomainObjectViewModel.DomainObject)
                                        .Where(d => d.GetType() == rootType)
                                        .OfType<IIndexable>()
                                        .OrderBy(d => d.Index)
                                        .ToList();

         IndexableCollectionExtension.Reindex(indexableCollection);

         foreach (var indexableDomainObject in indexableCollection)
         {
            var domainObjectService = (dynamic)_domainObjectServiceFactory.CreateDomainObjectService(indexableDomainObject.GetType());
            await domainObjectService.SaveIndexAsync(indexableDomainObject);
         }
      }

      #endregion
   }
}
