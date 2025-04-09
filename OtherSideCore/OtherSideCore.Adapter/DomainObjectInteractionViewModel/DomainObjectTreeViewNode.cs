using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectTreeViewNode<T> : ObservableObject, IDomainObjectTreeViewNode where T : DomainObject, new()
   {

      #region Fields

      private bool _isExpanded;
      private bool _isSelected;

      protected IDomainObjectInteractionService _domainObjectInteractionService;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected IUserDialogService _userDialogService;

      private DomainObjectViewModel _domainObjectViewModel;
      private IDomainObjectEditorViewModel _domainObjectEditorViewModel;
      private ObservableCollection<IDomainObjectTreeViewNode> _children;

      private IEnumerable<IDomainObjectTreeViewNode> _inlineNodes
      {
         get
         {
            foreach (var childNode in _children)
            {
               yield return childNode;

               foreach (var descendant in childNode.InlineNodes)
               {
                  yield return descendant;
               }
            }
         }
      }

      #endregion

      #region Properties

      public IEnumerable<IDomainObjectTreeViewNode> InlineNodes => _inlineNodes;

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public bool IsSelected
      {
         get => _isSelected;
         private set => SetProperty(ref _isSelected, value);
      }

      public DomainObjectViewModel DomainObjectViewModel
      {
         get => _domainObjectViewModel;
         private set => SetProperty(ref _domainObjectViewModel, value);
      }

      public IDomainObjectEditorViewModel DomainObjectEditorViewModel
      {
         get => _domainObjectEditorViewModel;
         private set => SetProperty(ref _domainObjectEditorViewModel, value);
      }

      public ObservableCollection<IDomainObjectTreeViewNode> Children
      {
         get => _children;
         private set => SetProperty(ref _children, value);
      }

      #endregion

      #region Events

      public event EventHandler<IDomainObjectTreeViewNode> NodeSelectionRequested;
      public event EventHandler<IDomainObjectTreeViewNode> NodeDeleted;
      public event EventHandler<DomainObject> ChildCreated;

      #endregion

      #region Commands

      public AsyncRelayCommand<IDomainObjectTreeViewNode> DupplicateChildNodeAsyncCommand { get; private set; }
      public AsyncRelayCommand ShowDomainObjectDetailsEditorAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel,
                                      IUserDialogService userDialogService,
                                      IDomainObjectInteractionService domainObjectInteractionService,
                                      IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         _domainObjectInteractionService = domainObjectInteractionService;
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _userDialogService = userDialogService;

         DomainObjectViewModel = domainObjectViewModel;

         DomainObjectEditorViewModel = domainObjectInteractionService.CreateDomainObjectEditorViewModel(domainObjectViewModel.DomainObject.GetType(), domainObjectViewModel);
         DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         DomainObjectEditorViewModel.DomainObjectDeletedEvent += DomainObjectEditorViewModel_DomainObjectDeleted;

         Children = new ObservableCollection<IDomainObjectTreeViewNode>();

         DupplicateChildNodeAsyncCommand = new AsyncRelayCommand<IDomainObjectTreeViewNode>(DupplicateChildNodeAsync, CanDupplicateChildNodeAsync);
         ShowDomainObjectDetailsEditorAsyncCommand = new AsyncRelayCommand(ShowDomainObjectDetailsEditorAsync, CanShowDomainObjectDetailsEditor);
      }

      #endregion

      #region Public Methods

      public virtual void AddChild(IDomainObjectTreeViewNode childNode, bool isInitializing)
      {
         DomainObjectTreeViewModelExtension.InsertNodeInList(childNode, Children);
      }

      public virtual void RemoveChild(IDomainObjectTreeViewNode childNode)
      {
         Children.Remove(childNode);
      }

      public void RequestSelection()
      {
         NodeSelectionRequested?.Invoke(this, this);
      }

      public void Select()
      {
         IsSelected = true;
         DomainObjectViewModel.IsSelected = true;
      }

      public void Unselect()
      {
         IsSelected = false;
         DomainObjectViewModel.IsSelected = false;
      }

      public void Expand()
      {
         IsExpanded = true;
      }

      public void Collapse()
      {
         IsExpanded = false;
      }

      public async Task<DomainObject> CreateChildNodeDomainObjectCopyAsync(IDomainObjectTreeViewNode node)
      {
         var dupplicatedDomainObject = await node.DomainObjectEditorViewModel.DupplicateAsync(DomainObjectEditorViewModel.DomainObjectViewModel.DomainObject);

         NotifyChildCreated(dupplicatedDomainObject);

         var dupplicatedDomainObjectNode = GetNode(dupplicatedDomainObject);

         foreach (var childNode in node.Children)
         {
            await dupplicatedDomainObjectNode.CreateChildNodeDomainObjectCopyAsync(childNode);
         }

         return dupplicatedDomainObject;
      }

      public virtual void Dispose()
      {
         DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         DomainObjectEditorViewModel.DomainObjectDeletedEvent -= DomainObjectEditorViewModel_DomainObjectDeleted;
         DomainObjectEditorViewModel.Dispose();
      }

      public void NotifyChildCreated(DomainObject domainObject)
      {
         ChildCreated?.Invoke(this, domainObject);
      }

      #endregion

      #region Private Methods

      protected IDomainObjectTreeViewNode GetNode(DomainObject domainObject)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.DomainObject.Equals(domainObject));
      }

      private void DomainObjectEditorViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(IDomainObjectEditorViewModel.HasUnsavedChanges)))
         {
            NotifyCommandsCanExecuteChanged();
         }
      }

      private void DomainObjectEditorViewModel_DomainObjectDeleted(object? sender, int e)
      {
         NodeDeleted?.Invoke(this, this);
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         DupplicateChildNodeAsyncCommand.NotifyCanExecuteChanged();
      }

      private bool CanDupplicateChildNodeAsync(IDomainObjectTreeViewNode? node)
      {
         return node != null && _inlineNodes.Contains(node);
      }

      protected virtual async Task<DomainObject> DupplicateChildNodeAsync(IDomainObjectTreeViewNode? node)
      {
         var domainObject = await CreateChildNodeDomainObjectCopyAsync(node);

         await IndexChildren(node.DomainObjectViewModel.DomainObject.GetType());

         return domainObject;
      }

      private async Task IndexChildren(Type domainObjectChildrenType)
      {
         var indexableCollection = Children.Select(c => c.DomainObjectViewModel.DomainObject)
                                           .Where(d => d.GetType() == domainObjectChildrenType)
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

      public bool CanShowDomainObjectDetailsEditor()
      {
         return !DomainObjectEditorViewModel.HasUnsavedChanges;
      }

      public virtual async Task ShowDomainObjectDetailsEditorAsync()
      {
         await _domainObjectInteractionService.DisplayDomainObjectDetailsEditorViewAsync(DomainObjectViewModel, DisplayType.Modal);
      }

      #endregion
   }
}
