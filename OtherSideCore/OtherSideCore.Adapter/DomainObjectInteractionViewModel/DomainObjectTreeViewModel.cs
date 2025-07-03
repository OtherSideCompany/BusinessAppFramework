using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Tree;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public abstract class DomainObjectTreeViewModel : ObservableObject, IDisposable, ISavable
   {
      #region Fields

      private readonly SemaphoreSlim _domainObjectEditorViewModelsSemaphore = new SemaphoreSlim(1, 1);


      private DomainObjectTree _domainObjectTree;
      protected DomainObjectTreeViewModelDependencies _domainObjectTreeViewModelDependencies;

      private ObservableCollection<IDomainObjectTreeNodeViewModel> _roots;
      private bool _isSelectionLocked;
      private IDomainObjectTreeNodeViewModel _selectedDomainObjectTreeViewNode;
      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;
      private DomainObjectViewModel _ContextViewModel;
      protected bool _isInitializingTree;

      private bool _isLoadingNestedStructures;

      protected IEnumerable<IDomainObjectTreeNodeViewModel> _inlineNodes
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

      public ObservableCollection<IDomainObjectTreeNodeViewModel> Roots
      {
         get => _roots;
         private set => SetProperty(ref _roots, value);
      }

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      public IDomainObjectTreeNodeViewModel? SelectedDomainObjectTreeViewNode
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

      public Type? DefaultRootType { get; set; }
      public Func<Type, Task<DomainObject>>? CreateRootDomainObjectAsync { get; set; }
      public Func<DomainObjectViewModel, IEnumerable<DomainObject>>? ResolveRootNodes { get; set; }
      public Action<DomainObjectViewModel, DomainObjectViewModel>? AttachRootNode { get; set; }
      public Action<DomainObjectViewModel, DomainObjectViewModel>? DetachRootNode { get; set; }

      #endregion

      #region Events

      public EventHandler PreviewTreeModified;
      public EventHandler TreeModified;

      #endregion

      #region Commands

      public AsyncRelayCommand CreateDefaultRootNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand<IDomainObjectTreeNodeViewModel> DupplicateRootNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectTreeViewModel(
         DomainObjectTree domainObjectTree,
         DomainObjectTreeViewModelDependencies domainObjectTreeViewModelDependencies)
      {

         _domainObjectTree = domainObjectTree;
         _domainObjectTreeViewModelDependencies = domainObjectTreeViewModelDependencies;

         Roots = new ObservableCollection<IDomainObjectTreeNodeViewModel>();

         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         CreateDefaultRootNodeAsyncCommand = new AsyncRelayCommand(CreateDefaultRootNodeAsync, CanCreateDefaultRootNode);
         DupplicateRootNodeAsyncCommand = new AsyncRelayCommand<IDomainObjectTreeNodeViewModel>(DupplicateRootNodeAsync, CanDupplicateRootNodeAsync);
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
         await _domainObjectTree.FillDomainObjectAsync(domainObjectViewModel.DomainObject);

         await _domainObjectEditorViewModelsSemaphore.WaitAsync();

         await ConstructTreeAsync(domainObjectViewModel);

         _domainObjectEditorViewModelsSemaphore.Release();

         _isInitializingTree = false;

         NotifyCommandsCanExecuteChanged();
      }

      public async Task SelectNodeAsync(IDomainObjectTreeNodeViewModel domainObjectTreeViewNode)
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

      public virtual async Task<IDomainObjectTreeNodeViewModel> AddRootNodeAsync(DomainObjectViewModel domainObjectViewModel)
      {
         if (ContextViewModel != null && AttachRootNode != null && !_isInitializingTree)
         {
            AttachRootNode(ContextViewModel, domainObjectViewModel);
         }

         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         var domainObjectRootTreeViewNode = await _domainObjectTreeViewModelDependencies.DomainObjectInteractionService.CreateDomainObjectTreeNodeViewModelAsync(domainObjectViewModel);

         DomainObjectTreeViewModelExtensions.InsertNodeInList(domainObjectRootTreeViewNode, Roots);

         RegisterNode(domainObjectRootTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);

         domainObjectRootTreeViewNode.Expand();

         return domainObjectRootTreeViewNode;
      }

      public async Task<IDomainObjectTreeNodeViewModel> AddChildNodeAsync(DomainObjectViewModel domainObjectViewModel, DomainObjectViewModel parentViewModel)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         var domainObjectTreeViewNode = await _domainObjectTreeViewModelDependencies.DomainObjectInteractionService.CreateDomainObjectTreeNodeViewModelAsync(domainObjectViewModel);
         var parentNode = GetNode(parentViewModel);
         parentNode.AddChild(domainObjectTreeViewNode, _isInitializingTree);
         RegisterNode(domainObjectTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);

         return domainObjectTreeViewNode;
      }

      public virtual void RemoveRootNode(IDomainObjectTreeNodeViewModel rootNodeToRemove)
      {
         PreviewTreeModified?.Invoke(this, EventArgs.Empty);

         UnregisterNode(rootNodeToRemove);
         Roots.Remove(rootNodeToRemove);

         TreeModified?.Invoke(this, EventArgs.Empty);

         if (ContextViewModel != null && DetachRootNode != null)
         {
            DetachRootNode(ContextViewModel, rootNodeToRemove.DomainObjectViewModel);
         }
      }

      public async Task RemoveChildNodeAsync(IDomainObjectTreeNodeViewModel nodeToRemove, IDomainObjectTreeNodeViewModel parent)
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

      public virtual async Task<DomainObject> CreateRootNodeDomainObjectCopyAsync(IDomainObjectTreeNodeViewModel node)
      {
         var dupplicatedDomainObject = await node.DomainObjectEditorViewModel.DupplicateAsync(ContextViewModel?.DomainObject);

         var dupplicatedDomainObjectViewModel = _domainObjectTreeViewModelDependencies.DomainObjectViewModelFactory.CreateViewModel(dupplicatedDomainObject);
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

      public IDomainObjectTreeNodeViewModel GetNode(DomainObjectViewModel domainObjectViewModel)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.Equals(domainObjectViewModel));
      }

      public IDomainObjectTreeNodeViewModel GetNode(DomainObject domainObject)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.DomainObject.Equals(domainObject));
      }

      public async Task<DomainObject> CreateTypedRootDomainObjectAsync(Type rootType)
      {
         DomainObject domainObject;

         var domainObjectService = (dynamic)_domainObjectTreeViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService(rootType);
         domainObject = await domainObjectService.CreateAsync(ContextViewModel.DomainObject);

         await IndexRoots(rootType);

         return domainObject;
      }

      public virtual void Dispose()
      {
         Clear();
      }

      #endregion

      #region Private Methods  

      protected virtual void RegisterNode(IDomainObjectTreeNodeViewModel domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested += DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted += DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated += DomainObjectTreeViewNode_ChildCreated;
      }

      protected virtual void UnregisterNode(IDomainObjectTreeNodeViewModel domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested -= DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted -= DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated -= DomainObjectTreeViewNode_ChildCreated;
      }

      protected virtual async void DomainObjectTreeViewNode_ChildCreated(object? sender, Domain.DomainObjects.DomainObject e)
      {
         var viewModel = _domainObjectTreeViewModelDependencies.DomainObjectViewModelFactory.CreateViewModel(e);
         await AddChildNodeAsync(viewModel, ((IDomainObjectTreeNodeViewModel)sender).DomainObjectViewModel);
      }

      protected virtual async void DomainObjectTreeViewNode_NodeDeleted(object? sender, IDomainObjectTreeNodeViewModel e)
      {
         await RemoveNodeAsync(e);
      }

      private async Task RemoveNodeAsync(IDomainObjectTreeNodeViewModel nodeToRemove)
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

      private async Task RemoveNodeFromChildrenAsync(IDomainObjectTreeNodeViewModel nodeToRemove, IDomainObjectTreeNodeViewModel parent)
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

      private async void DomainObjectRootTreeViewNode_NodeSelected(object? sender, IDomainObjectTreeNodeViewModel e)
      {
         await SelectNodeAsync(e);
      }

      protected async Task ConstructTreeAsync(DomainObjectViewModel context)
      {
         Clear();

         if (ResolveRootNodes == null)
         {
            throw new InvalidOperationException("ResolveRootNodesAsync must be set.");
         }

         foreach (var domainObject in ResolveRootNodes(context))
         {
            var viewModel = _domainObjectTreeViewModelDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
            await AddRootNodeAsync(viewModel);
         }
      }


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
         CreateDefaultRootNodeAsyncCommand.NotifyCanExecuteChanged();
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }

      private bool CanCreateDefaultRootNode()
      {
         return ContextViewModel != null && DefaultRootType != null;
      }

      protected virtual async Task<IDomainObjectTreeNodeViewModel> CreateDefaultRootNodeAsync()
      {
         var domainObject = await CreateDefaultRootRootDomainObjectAsync();

         var viewModel = _domainObjectTreeViewModelDependencies.DomainObjectViewModelFactory.CreateViewModel(domainObject);
         return await AddRootNodeAsync(viewModel);
      }

      private bool CanDupplicateRootNodeAsync(IDomainObjectTreeNodeViewModel? node)
      {
         return node != null && Roots.Contains(node);
      }

      protected virtual async Task<DomainObject> DupplicateRootNodeAsync(IDomainObjectTreeNodeViewModel? node)
      {
         var domainObject = await CreateRootNodeDomainObjectCopyAsync(node);

         await IndexRoots(node.DomainObjectViewModel.DomainObject.GetType());

         return domainObject;
      }

      protected virtual async Task<DomainObject> CreateDefaultRootRootDomainObjectAsync()
      {
         if (DefaultRootType != null)
         {
            return await CreateTypedRootDomainObjectAsync(DefaultRootType);
         }
         else
         {
            throw new InvalidOperationException("DefaultRootType must be set to create a root node domain object.");
         }
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
            var domainObjectService = (dynamic)_domainObjectTreeViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService(indexableDomainObject.GetType());
            await domainObjectService.SaveIndexAsync(indexableDomainObject);
         }
      }

      #endregion
   }
}
