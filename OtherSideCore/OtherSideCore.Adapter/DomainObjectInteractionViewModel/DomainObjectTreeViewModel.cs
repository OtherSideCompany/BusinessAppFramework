using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public abstract class DomainObjectTreeViewModel : UIInteractionHost, IDisposable
   {
      #region Fields

      private IDomainObjectInteractionFactory _domainObjectInteractionFactory;
      private IDomainObjectTreeSearchViewModel _domainObjectTreeSearchViewModel;
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;

      private ObservableCollection<IDomainObjectTreeViewNode> _roots;
      private bool _isSelectionLocked;
      private IDomainObjectTreeViewNode _selectedDomainObjectTreeViewNode;
      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;
      private DomainObjectViewModel _ContextViewModel;
      protected bool _isInitializingTree;

      private bool _isLoadingNestedBrowsers;

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

      public IDomainObjectTreeViewNode SelectedDomainObjectTreeViewNode
      {
         get => _selectedDomainObjectTreeViewNode;
         private set => SetProperty(ref _selectedDomainObjectTreeViewNode, value);
      }

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel => SelectedDomainObjectTreeViewNode?.DomainObjectEditorViewModel;

      public bool IsLoadingNestedBrowsers
      {
         get => _isLoadingNestedBrowsers;
         private set => SetProperty(ref _isLoadingNestedBrowsers, value);
      }

      public DomainObjectViewModel ContextViewModel
      {
         get => _ContextViewModel;
         set => SetProperty(ref _ContextViewModel, value);
      }

      public bool HasUnsavedChanges => _inlineNodes.Select(n => n.DomainObjectEditorViewModel).Any(vm => vm.HasUnsavedChanges);

      #endregion

      #region Events

      public EventHandler TreeModified;

      #endregion

      #region Commands

      public AsyncRelayCommand CreateRootNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }


      #endregion

      #region Constructor

      public DomainObjectTreeViewModel(IUserDialogService userDialogService,
                                       IWindowService windowService,
                                       IDomainObjectInteractionFactory domainObjectInteractionFactory,
                                       IDomainObjectTreeSearchViewModel domainObjectTreeSearchViewModel,
                                       IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                       IDomainObjectServiceFactory domainObjectServiceFactory) :
       base(userDialogService, windowService)
      {
         _domainObjectInteractionFactory = domainObjectInteractionFactory;
         _domainObjectTreeSearchViewModel = domainObjectTreeSearchViewModel;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;

         Roots = new ObservableCollection<IDomainObjectTreeViewNode>();

         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         CreateRootNodeAsyncCommand = new AsyncRelayCommand(CreateRootNodeAsync, CanCreateRootNode);
      }

      #endregion

      #region Public Methods

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         _inlineNodes.Select(n => n.DomainObjectEditorViewModel).ToList().ForEach(async vm => await vm.SaveChangesAsync());
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         _inlineNodes.Select(n => n.DomainObjectEditorViewModel).ToList().ForEach(async vm => await vm.CancelChangesAsync());
      }

      public async Task InitializeAsync(DomainObjectViewModel domainObjectViewModel)
      {
         _isInitializingTree = true;

         ContextViewModel = domainObjectViewModel;
         await _domainObjectTreeSearchViewModel.SearchAsync(domainObjectViewModel);
         await ConstructTreeAsync(domainObjectViewModel);

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

               IsLoadingNestedBrowsers = true;

               await SelectedDomainObjectEditorViewModel.LoadNestedStructuresAsync();

               IsLoadingNestedBrowsers = false;
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

      public virtual void Dispose()
      {
         Clear();
      }

      #endregion

      #region Private Methods

      protected virtual void AddRootNode(DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectRootTreeViewNode = _domainObjectInteractionFactory.CreateDomainObjectTreeViewNode(domainObjectViewModel);
         Roots.Add(domainObjectRootTreeViewNode);
         RegisterNode(domainObjectRootTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      protected void AddChildNode(DomainObjectViewModel domainObjectViewModel, DomainObjectViewModel parentViewModel)
      {
         var domainObjectTreeViewNode = _domainObjectInteractionFactory.CreateDomainObjectTreeViewNode(domainObjectViewModel);
         var parentNode = GetNode(parentViewModel);
         parentNode.AddChild(domainObjectTreeViewNode, _isInitializingTree);
         RegisterNode(domainObjectTreeViewNode);

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      protected virtual void RemoveRootNode(IDomainObjectTreeViewNode rootNodeToRemove)
      {
         UnregisterNode(rootNodeToRemove);
         Roots.Remove(rootNodeToRemove);
         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      protected void RemoveChildNode(IDomainObjectTreeViewNode nodeToRemove, IDomainObjectTreeViewNode parent)
      {
         UnregisterNode(nodeToRemove);
         parent.RemoveChild(nodeToRemove);
         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      protected void Clear()
      {
         _inlineNodes.ToList().ForEach(n => UnregisterNode(n));
         _inlineNodes.ToList().ForEach(n => n.Dispose());
         Roots.Clear();

         TreeModified?.Invoke(this, EventArgs.Empty);
      }

      private void RegisterNode(IDomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested += DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted += DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated += DomainObjectTreeViewNode_ChildCreated;
      }

      private void UnregisterNode(IDomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         domainObjectTreeViewNode.DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         domainObjectTreeViewNode.NodeSelectionRequested -= DomainObjectRootTreeViewNode_NodeSelected;
         domainObjectTreeViewNode.NodeDeleted -= DomainObjectTreeViewNode_NodeDeleted;
         domainObjectTreeViewNode.ChildCreated -= DomainObjectTreeViewNode_ChildCreated;
      }

      private void DomainObjectTreeViewNode_ChildCreated(object? sender, Domain.DomainObjects.DomainObject e)
      {
         var viewModel = _domainObjectViewModelFactory.CreateViewModel(e);
         AddChildNode(viewModel, ((IDomainObjectTreeViewNode)sender).DomainObjectViewModel);
      }

      private void DomainObjectTreeViewNode_NodeDeleted(object? sender, IDomainObjectTreeViewNode e)
      {
         RemoveNode(e);
      }     

      private void RemoveNode(IDomainObjectTreeViewNode nodeToRemove)
      {
         foreach (var rootNode in Roots.ToList())
         {
            if (rootNode.Equals(nodeToRemove))
            {
               RemoveRootNode(nodeToRemove);               
               return;
            }

            RemoveNodeFromChildren(nodeToRemove, rootNode);
         }
      }

      private void RemoveNodeFromChildren(IDomainObjectTreeViewNode nodeToRemove, IDomainObjectTreeViewNode parent)
      {
         foreach (var childNode in parent.Children.ToList())
         {
            if (childNode.Equals(nodeToRemove))
            {
               RemoveChildNode(nodeToRemove, parent);               
               return;
            }

            RemoveNodeFromChildren(nodeToRemove, childNode);
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

      protected IDomainObjectTreeViewNode GetNode(DomainObjectViewModel domainObjectViewModel)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.Equals(domainObjectViewModel));
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

      protected virtual async Task CreateRootNodeAsync()
      {
         var domainObject = await CreateRootDomainObjectAsync();
            
         var viewModel = _domainObjectViewModelFactory.CreateViewModel(domainObject);
         AddRootNode(viewModel);
      }

      protected virtual async Task<DomainObject> CreateRootDomainObjectAsync()
      {
         return null;
      }

      #endregion
   }
}
