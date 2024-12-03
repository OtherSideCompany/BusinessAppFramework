using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectTreeViewNode<T> : UIInteractionHost, IDomainObjectTreeViewNode where T : DomainObject, new()
   {

      #region Fields

      private bool _isExpanded;
      private bool _isSelected;

      protected IDomainObjectInteractionFactory _domainObjectInteractionFactory;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;

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

      public AsyncRelayCommand DeleteAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel,
                                      IUserDialogService userDialogService,
                                      IWindowService windowService,
                                      IDomainObjectInteractionFactory domainObjectInteractionFactory,
                                      IDomainObjectServiceFactory domainObjectServiceFactory) :
         base(userDialogService, windowService)
      {
         _domainObjectInteractionFactory = domainObjectInteractionFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;

         DomainObjectViewModel = domainObjectViewModel;

         DomainObjectEditorViewModel = domainObjectInteractionFactory.CreateDomainObjectEditorViewModel(domainObjectViewModel.DomainObject.GetType(), domainObjectViewModel);
         DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;

         Children = new ObservableCollection<IDomainObjectTreeViewNode>();

         DeleteAsyncCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
      }      

      #endregion

      #region Public Methods

      public virtual void AddChild(IDomainObjectTreeViewNode childNode, bool isInitializing)
      {
         Children.Add(childNode);
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

      public void Dispose()
      {
         DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         DomainObjectEditorViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      private void DomainObjectEditorViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(IDomainObjectEditorViewModel.HasUnsavedChanges)))
         {
            NotifyCommandsCanExecuteChanged();
         }
      }

      protected virtual bool CanDelete()
      {
         return !DomainObjectEditorViewModel.HasUnsavedChanges;
      }

      protected virtual async Task DeleteAsync()
      {
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            var domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();

            await domainObjectService.DeleteAsync((T)DomainObjectViewModel.DomainObject);

            NodeDeleted?.Invoke(this, this);
         }
      }

      protected void NotifyChildCreated(DomainObject domainObject)
      {
         ChildCreated?.Invoke(this, domainObject);
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         DeleteAsyncCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
